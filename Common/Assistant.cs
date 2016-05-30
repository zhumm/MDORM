using System;
using System.Configuration;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace Common
{
	/// <summary>
	/// 辅助类库
	/// </summary>
	public sealed class Assistant
	{
        private static List<string> _randomList = new List<string>();

        /// <summary>
        /// Gets the ranom list.
        /// </summary>
        /// 创建人：朱明明
        /// 创建时间：2014-7-25 15:13
        public static List<string> RanomList
        {
            get
            {
                if (_randomList == null || _randomList.Count <= 0)
                {
                    _randomList.AddRange(new string[]{
					                     	"A","B","C","D","E","F","G",
					                     	"H","I","J","K","L","M","N",
					                     	"O","P","Q",
					                     	"R","S","T",
					                     	"U","V","W",
					                     	"X","Y","Z",
					                     	"0","1","2","3","4","5","6","7","8","9"
					                     });
                }
                return _randomList;
            }
        }

        /// <summary>
        /// 生成一定位数的随机数
        /// </summary>
        /// <param name="num">生成随机数的位数</param>
        /// <returns></returns>
        private static string CreateRandomNum(int num)
        {
            StringBuilder randomContent = new StringBuilder();
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < num; i++)
            {
                randomContent.Append(RanomList[rd.Next(0, 36)]);
            }

            return randomContent.ToString();
        }

		#region
		/// <summary>
		/// 从字符串里随机得到，规定个数的字符串.
		/// </summary>
		/// <param name="allChar"></param>
		/// <param name="CodeCount"></param>
		/// <returns></returns>
		private string GetRandomCode(string allChar,int CodeCount) 
		{ 
			//string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"; 
			string[] allCharArray = allChar.Split(','); 
			string RandomCode = ""; 
			int temp = -1; 
			Random rand = new Random(); 
			for (int i=0;i<CodeCount;i++) 
			{ 
				if (temp != -1) 
				{ 
					rand = new Random(temp*i*((int) DateTime.Now.Ticks)); 
				} 

				int t = rand.Next(allCharArray.Length-1); 
				while (temp == t) 
				{ 
					t = rand.Next(allCharArray.Length-1); 
				} 
		
				temp = t; 
				RandomCode += allCharArray[t]; 
			} 
			return RandomCode; 
		}

		#endregion
	}
}
