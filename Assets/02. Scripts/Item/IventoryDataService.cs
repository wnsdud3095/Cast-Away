using System.IO;
using System;
using UnityEngine;
namespace InventoryService
{
    public class IventoryDataService : ISaveable, IInventoryService
    {
        private IItemDataBase m_item_db;

        private int m_money;
        private ItemData[] m_items;

        public event Action<int, ItemData> OnUpdatedSlot;

        public int Gold => m_money;

        public IventoryDataService()
        {
            m_money = 0;
            m_items = new ItemData[12];
            for (int i = 0; i < m_items.Length; i++)
            {
                m_items[i] = new ItemData();
            }

            // 디렉터리 경로가 없다면 새롭게 생성한다.
            CreateDirectory();
        }

        private void CreateDirectory()
        {
            var directory_path = Path.Combine(Application.persistentDataPath, "Inventory");

            if (!Directory.Exists(directory_path))
            {
                Directory.CreateDirectory(directory_path);
#if UNITY_EDITOR
                Debug.Log($"<color=cyan>Inventory 디렉터리를 새롭게 생성합니다.</color>");
#endif
            }
        }

        // Inject()를 통해서 아이템 매니저를 주입받는다.
        public void Inject(IItemDataBase item_db)
        {
            m_item_db = item_db;
        }

        // offset에 해당하는 슬롯을 향하여 이벤트를 발생시킨다.
        public void InitializeSlot(int offset)
        {
            OnUpdatedSlot?.Invoke(offset, m_items[offset]);
        }


        // 아이템을 획득할 때 사용한다.
        public void AddItem(ItemCode code, int count)
        {
            var item = m_item_db.GetItem(code);

            // 아이템이 중첩 가능하다면
            if (item.Stackable)
            {
                // 슬롯들을 순회하면서
                for (int i = 0; i < m_items.Length; i++)
                {
                    // 아이템 코드가 일치하면서 99개 이하인 슬롯을 찾는다.
                    if (m_items[i].Code == code && m_items[i].Count + count <= 99)
                    {
                        m_items[i].Count += count;

                        OnUpdatedSlot?.Invoke(i, m_items[i]);
                        return;
                    }
                }
            }

            // 중첩 아이템이 아니라면
            for (int i = 0; i < m_items.Length; i++)
            {
                // 비어있는 슬롯을 찾는다.
                if (m_items[i].Code == ItemCode.NONE)
                {
                    m_items[i].Code = code;
                    m_items[i].Count = count;

                    OnUpdatedSlot?.Invoke(i, m_items[i]);
                    return;
                }
            }
        }

        // 아이템을 제거할 때 사용한다.
        public void RemoveItem(ItemCode code, int count)
        {
            var item = m_item_db.GetItem(code);

            // 아이템이 중첩 가능하다면
            if (item.Stackable)
            {
                // 아무래도 뒤에서부터 순회해야 99개가 아닐 확률이 높다.
                for (int i = m_items.Length - 1; i >= 0; i--)
                {
                    // 해당 아이템이 들어있는 슬롯을 발견했다면
                    if (m_items[i].Code == code)
                    {
                        // count 만큼 제거할 수 있는지 확인하고
                        if (m_items[i].Count >= count)
                        {
                            // 제거 가능하다면 제거하고
                            m_items[i].Count -= count;

                            // 제거하려는 개수와 같아서 슬롯의 아이템 개수가 0이라면
                            if (m_items[i].Count == 0)
                            {
                                // 슬롯을 비운다.
                                Clear(i);
                            }

                            OnUpdatedSlot?.Invoke(i, m_items[i]);
                            return;
                        }
                        else
                        {
                            // 제거할 수 없다면 제거할 수 있는 만큼만 제거하고 슬롯을 비운다.
                            count -= m_items[i].Count;
                            Clear(i);
                        }
                    }
                }
            }

            // 중첩 아이템이 아니라면
            for (int i = 0; i < m_items.Length; i++)
            {
                // 아이템 코드가 일치하는 슬롯을
                if (m_items[i].Code == code)
                {
                    // 비운다.
                    Clear(i);

                    OnUpdatedSlot?.Invoke(i, m_items[i]);
                    return;
                }
            }
        }

        // 아이템을 원하는 위치에 설정하고 싶을 때 사용한다.
        public void SetItem(int offset, ItemCode code, int count)
        {
            // offset 위치의 슬롯에 code와 count만큼을 채운다.
            m_items[offset].Code = code;
            m_items[offset].Count = count;

            OnUpdatedSlot?.Invoke(offset, m_items[offset]);
        }

        // 원하는 위치의 아이템의 개수를 갱신하고 싶을 때 사용한다.
        public int UpdateItem(int offset, int count)
        {
            // 슬롯의 최대 보관 개수 이하라면 -1을 반환하고,
            if (m_items[offset].Count + count <= 99)
            {
                m_items[offset].Count += count;
                OnUpdatedSlot?.Invoke(offset, m_items[offset]);

                return -1;
            }
            else    // 그게 아니라면 저장할 만큼만 저장한다.
            {
                var remain_count = 99 - m_items[offset].Count;

                m_items[offset].Count = 99;
                OnUpdatedSlot?.Invoke(offset, m_items[offset]);

                return remain_count;
            }
        }

        // 특정 위치의 슬롯을 비운다.
        public void Clear(int offset)
        {
            m_items[offset].Code = ItemCode.NONE;
            m_items[offset].Count = 0;

            OnUpdatedSlot?.Invoke(offset, m_items[offset]);
        }

        // 특정 아이템의 총 개수를 반환한다.
        public int GetItemCount(ItemCode code)
        {
            var total_count = 0;

            foreach (var slot in m_items)
            {
                if (slot.Code == code)
                {
                    total_count += slot.Count;
                }
            }

            return total_count;
        }

        // 아이템을 저장할 수 있는 타당한 위치를 반환한다.
        public int GetValidOffset(ItemCode code)
        {
            for (int offset = 0; offset < m_items.Length; offset++)
            {
                var item = m_item_db.GetItem(code);
                if (item.Stackable)
                {
                    if (m_items[offset].Count < 99)
                    {
                        return offset;
                    }
                }

                if (m_items[offset].Code == ItemCode.NONE)
                {
                    return offset;
                }
            }

            return -1;
        }

        // 아이템을 우선적으로 제거할 우선순위 슬롯을 반환한다.
        public int GetPriorityOffset(ItemCode code)
        {
            for (int offset = m_items.Length - 1; offset >= 0; offset--)
            {
                if (m_items[offset].Code == code)
                {
                    return offset;
                }
            }

            return -1;
        }

        // 아이템 보유 여부를 반환한다.
        public bool HasItem(ItemCode code)
        {
            foreach (var slot in m_items)
            {
                if (slot.Code == code)
                {
                    return true;
                }
            }

            return false;
        }

        // 특정 위치의 아이템 데이터를 반환한다.
        public ItemData GetItem(int offset)
        {
            return m_items[offset];
        }
        public bool Load()
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Inventory", $"InventoryData.json");

            if (File.Exists(local_data_path))
            {
                var json_data = File.ReadAllText(local_data_path);
                var inventory_data = JsonUtility.FromJson<InventoryData>(json_data);

                m_items = inventory_data.Items;
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Save()
        {
            var local_data_path = Path.Combine(Application.persistentDataPath, "Inventory", $"InventoryData.json");

            var inventory_data = new InventoryData(m_items);
            var json_data = JsonUtility.ToJson(inventory_data, true);

            File.WriteAllText(local_data_path, json_data);
        }
    }
}