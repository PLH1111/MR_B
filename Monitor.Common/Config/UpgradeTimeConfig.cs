using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Common
{
    public class UpgradeTimeConfig
    {
        //public static Dictionary<int, string>[] ErrorCode;

        public int StartTimeout            { get; set; } = 1000;
        public int StartTimes              { get; set; } = 30;
        public int TranPacketDelayTime     { get; set; } = 20;
        public int ProcessInterval         { get; set; } = 2000;
        public int CommunicationTimeout    { get; set; } = 3000;
        public int CommunicationRetryTimes { get; set; } = 3;
        public int CheckProgressTimeout    { get; set; } = 60000;
    }

    public enum ErrorCodeEnum
    {
        SUCCESS           = 0,
        ERR_FUN_CODE      = 1,
        ERR_ADDR          = 2,
        ERR_DATA          = 3,
        ERR_CRC           = 4,
        ERR_OUT_OF_RANGE  = 5,
        ERR_TIMEOUT       = 6,
        ERR_WRITE_FAIL    = 7,
        ERR_FUN_UNSUPPORT = 8,

        ERR_PROJECT_INFO        = 0xE0,
        ERR_BOOT_VER            = 0xE1,
        ERR_FILE_SIZE           = 0xE2,
        ERR_UPG_FILE_CRC        = 0xE3,
        ERR_BACKUP_FILE_CRC     = 0xE4,
        ERR_ERASE               = 0xE5,
        ERR_BLOCK_DISCONTINUOUS = 0xE6,
        ERR_WRITE_INT_FLASH     = 0xE7,
        ERR_WRITE_EXT_FLASH     = 0xE8,

        ERROR = 0xFD,
        BUSY  = 0xFE,
        IDLE  = 0xFF
    }
}
