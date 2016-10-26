using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEl
{
    public enum EDMStatus
    {
        /// <summary>
        /// EDM发送成功
        /// </summary>
        EDMSendSuccess = 1,
        /// <summary>
        /// EDM主题为空
        /// </summary>
        EDMSubjectEmpty = 2,
        /// <summary>
        /// EDM内容为空
        /// </summary>
        EDMContentEmpty = 3,
        /// <summary>
        /// EDM收件人为空
        /// </summary>
        EDMToAddressEmpty = 4,
        /// <summary>
        /// EDM其他异常错误
        /// </summary>
        EDMException = 5,
        /// <summary>
        /// EDM发送失败
        /// </summary>
        EDMSendFailed = 6,
        /// <summary>
        /// EDM不需发送
        /// </summary>
        EDMNotSend = 7,

    }
}
