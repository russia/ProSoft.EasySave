﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSoft.EasySave.Application.Enums
{
    public enum TransferType
    {
        /// <summary>
        /// Copy and replace the files if they exist
        /// </summary>
        FULL,

        /// <summary>
        /// Copies only files that do not exist in the destination folder
        /// </summary>
        DIFFERENTIAL
    }
}