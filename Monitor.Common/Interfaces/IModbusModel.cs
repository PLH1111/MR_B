namespace Monitor.Common
{
     public interface IModbusModel
     {
         /// <summary>
         /// 完整发送数据
         /// </summary>
         byte[] SendData          { get; } 

         /// <summary>
         /// 接收有效数据
         /// </summary>
         byte[] ReceiveValidBytes { get; }

       /// <summary>
       /// 初始化
       /// </summary>
       /// <param name="isRead"></param>
       /// <param name="data"></param>
        void InitSendBytes(bool isRead, byte[] data);

        /// <summary>
        /// 校验接收数据
        /// </summary>
        /// <param name="receive">返回数据</param>
        /// <param name="result">校验结果</param>
        /// <returns>是否停止校验</returns>
        bool CheckReceive(byte[] receive, out string result);

     }
}