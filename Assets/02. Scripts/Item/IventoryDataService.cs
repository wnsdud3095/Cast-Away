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

            // ���͸� ��ΰ� ���ٸ� ���Ӱ� �����Ѵ�.
            CreateDirectory();
        }

        private void CreateDirectory()
        {
            var directory_path = Path.Combine(Application.persistentDataPath, "Inventory");

            if (!Directory.Exists(directory_path))
            {
                Directory.CreateDirectory(directory_path);
#if UNITY_EDITOR
                Debug.Log($"<color=cyan>Inventory ���͸��� ���Ӱ� �����մϴ�.</color>");
#endif
            }
        }

        // Inject()�� ���ؼ� ������ �Ŵ����� ���Թ޴´�.
        public void Inject(IItemDataBase item_db)
        {
            m_item_db = item_db;
        }

        // offset�� �ش��ϴ� ������ ���Ͽ� �̺�Ʈ�� �߻���Ų��.
        public void InitializeSlot(int offset)
        {
            OnUpdatedSlot?.Invoke(offset, m_items[offset]);
        }


        // �������� ȹ���� �� ����Ѵ�.
        public void AddItem(ItemCode code, int count)
        {
            var item = m_item_db.GetItem(code);

            // �������� ��ø �����ϴٸ�
            if (item.Stackable)
            {
                // ���Ե��� ��ȸ�ϸ鼭
                for (int i = 0; i < m_items.Length; i++)
                {
                    // ������ �ڵ尡 ��ġ�ϸ鼭 99�� ������ ������ ã�´�.
                    if (m_items[i].Code == code && m_items[i].Count + count <= 99)
                    {
                        m_items[i].Count += count;

                        OnUpdatedSlot?.Invoke(i, m_items[i]);
                        return;
                    }
                }
            }

            // ��ø �������� �ƴ϶��
            for (int i = 0; i < m_items.Length; i++)
            {
                // ����ִ� ������ ã�´�.
                if (m_items[i].Code == ItemCode.NONE)
                {
                    m_items[i].Code = code;
                    m_items[i].Count = count;

                    OnUpdatedSlot?.Invoke(i, m_items[i]);
                    return;
                }
            }
        }

        // �������� ������ �� ����Ѵ�.
        public void RemoveItem(ItemCode code, int count)
        {
            var item = m_item_db.GetItem(code);

            // �������� ��ø �����ϴٸ�
            if (item.Stackable)
            {
                // �ƹ����� �ڿ������� ��ȸ�ؾ� 99���� �ƴ� Ȯ���� ����.
                for (int i = m_items.Length - 1; i >= 0; i--)
                {
                    // �ش� �������� ����ִ� ������ �߰��ߴٸ�
                    if (m_items[i].Code == code)
                    {
                        // count ��ŭ ������ �� �ִ��� Ȯ���ϰ�
                        if (m_items[i].Count >= count)
                        {
                            // ���� �����ϴٸ� �����ϰ�
                            m_items[i].Count -= count;

                            // �����Ϸ��� ������ ���Ƽ� ������ ������ ������ 0�̶��
                            if (m_items[i].Count == 0)
                            {
                                // ������ ����.
                                Clear(i);
                            }

                            OnUpdatedSlot?.Invoke(i, m_items[i]);
                            return;
                        }
                        else
                        {
                            // ������ �� ���ٸ� ������ �� �ִ� ��ŭ�� �����ϰ� ������ ����.
                            count -= m_items[i].Count;
                            Clear(i);
                        }
                    }
                }
            }

            // ��ø �������� �ƴ϶��
            for (int i = 0; i < m_items.Length; i++)
            {
                // ������ �ڵ尡 ��ġ�ϴ� ������
                if (m_items[i].Code == code)
                {
                    // ����.
                    Clear(i);

                    OnUpdatedSlot?.Invoke(i, m_items[i]);
                    return;
                }
            }
        }

        // �������� ���ϴ� ��ġ�� �����ϰ� ���� �� ����Ѵ�.
        public void SetItem(int offset, ItemCode code, int count)
        {
            // offset ��ġ�� ���Կ� code�� count��ŭ�� ä���.
            m_items[offset].Code = code;
            m_items[offset].Count = count;

            OnUpdatedSlot?.Invoke(offset, m_items[offset]);
        }

        // ���ϴ� ��ġ�� �������� ������ �����ϰ� ���� �� ����Ѵ�.
        public int UpdateItem(int offset, int count)
        {
            // ������ �ִ� ���� ���� ���϶�� -1�� ��ȯ�ϰ�,
            if (m_items[offset].Count + count <= 99)
            {
                m_items[offset].Count += count;
                OnUpdatedSlot?.Invoke(offset, m_items[offset]);

                return -1;
            }
            else    // �װ� �ƴ϶�� ������ ��ŭ�� �����Ѵ�.
            {
                var remain_count = 99 - m_items[offset].Count;

                m_items[offset].Count = 99;
                OnUpdatedSlot?.Invoke(offset, m_items[offset]);

                return remain_count;
            }
        }

        // Ư�� ��ġ�� ������ ����.
        public void Clear(int offset)
        {
            m_items[offset].Code = ItemCode.NONE;
            m_items[offset].Count = 0;

            OnUpdatedSlot?.Invoke(offset, m_items[offset]);
        }

        // Ư�� �������� �� ������ ��ȯ�Ѵ�.
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

        // �������� ������ �� �ִ� Ÿ���� ��ġ�� ��ȯ�Ѵ�.
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

        // �������� �켱������ ������ �켱���� ������ ��ȯ�Ѵ�.
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

        // ������ ���� ���θ� ��ȯ�Ѵ�.
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

        // Ư�� ��ġ�� ������ �����͸� ��ȯ�Ѵ�.
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