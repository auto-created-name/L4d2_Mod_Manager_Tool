using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L4d2_Mod_Manager_Tool.Widget
{
    /// <summary>
    /// 对ListView的列进行比较排序
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        private int ColumnToSort;  //指定按照哪列排序
        private SortOrder OrderOfSort;  //指定排序的方式
        private CaseInsensitiveComparer ObjectCompare;  //声明CaaseInsensitiveComparer类对象
        public ListViewColumnSorter()  //构造函数
        {
            ColumnToSort = 0;  //默认按第一列排序
            OrderOfSort = SortOrder.None;  //排序
            ObjectCompare = new CaseInsensitiveComparer();  //初始化CaseInsensitiveComparer类对象
        }
        //重写IComparer接口
        //返回比较的结果:如果x=y返回0；如果x>y返回1；如果x<y返回-1
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listViewX, listViewY;
            //将比较对象转换为ListViewItem对象
            listViewX = (ListViewItem)x;
            listViewY = (ListViewItem)y;
            //比较
            compareResult = ObjectCompare.Compare(listViewX.SubItems[ColumnToSort].Text, listViewY.SubItems[ColumnToSort].Text);
            // 返回比较的结果
            if (OrderOfSort == SortOrder.Ascending)
            {
                // 因为是正序排序，所以直接返回结果
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // 如果是反序排序，所以要取负值再返回
                return (-compareResult);
            }
            else
            {
                return 0;
            }
        }
        /// 获取并设置按照哪一列排序. 
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }
        /// 获取并设置排序方式.
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }
}
