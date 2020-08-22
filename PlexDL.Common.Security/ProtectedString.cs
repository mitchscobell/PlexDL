﻿using PlexDL.Common.Enums;
using System;

//using PlexDL.Common.Logging;

namespace PlexDL.Common.Security
{
    //This uses the Windows Data Protection API for the Current User to encrypt/decrypt strings.
    //It's near-impossible to decrypt this information if not logged in as the user that encrypted the original data.
    public class ProtectedString : ProtectedData
    {
        public string RawValue { get; set; }
        public StringProtectionMode Mode { get; set; }

        public string ProcessedValue => ProcessRawValue();

        public ProtectedString(string data, StringProtectionMode mode)
        {
            Mode = mode;
            RawValue = data;
        }

        private string ProcessRawValue()
        {
            var value = "";

            try
            {
                switch (Mode)
                {
                    case StringProtectionMode.Decrypt:
                        value = DecryptData(RawValue);
                        break;

                    case StringProtectionMode.Encrypt:
                        value = EncryptData(RawValue);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception)
            {
                //ignore the error
            }

            return value;
        }
    }
}