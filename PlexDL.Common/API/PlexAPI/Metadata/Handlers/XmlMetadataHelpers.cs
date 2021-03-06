﻿using PlexDL.Common.API.PlexAPI.Metadata.Objects;
using PlexDL.Common.Enums;
using PlexDL.Common.Globals;
using PlexDL.Common.Logging;
using PlexDL.Common.Net;
using System;
using System.Data;
using System.Xml;
using UIHelpers;

namespace PlexDL.Common.API.PlexAPI.Metadata.Handlers
{
    public static class XmlMetadataHelpers
    {
        public static XmlMetadata GetMetadata(DataRow result,
            string msgNoKey = "Error occurred whilst getting the unique content key",
            string logNoKeyMsg = "Error occurred whilst getting the unique content key",
            string logNoKeyType = "NoUnqKeyError", string column = "key")
        {
            var obj = new XmlMetadata();
            XmlDocument reply = null;

            var key = "";

            if (result[column] != null)
                if (!Equals(result[column], string.Empty))
                    key = result[column].ToString();

            if (string.IsNullOrEmpty(key))
            {
                UIMessages.Error(msgNoKey, @"Data Error");
                LoggingHelpers.RecordGeneralEntry("Unique key error");
                LoggingHelpers.RecordException(logNoKeyMsg, logNoKeyType);
            }
            else
            {
                var baseUri = Strings.GetBaseUri(false);
                key = key.TrimStart('/');
                var uri = baseUri + key + "/?X-Plex-Token=";

                LoggingHelpers.RecordGeneralEntry("Contacting the API");
                reply = XmlGet.GetXmlTransaction(uri);
            }

            obj.Xml = reply;
            obj.ApiUri = $"/{key}";
            return obj;
        }

        public static string GetContentAttribute(XmlDocument metadata, ContentType type, string attributeName,
            string defaultValue = @"Unknown")
        {
            switch (type)
            {
                case ContentType.Movies:
                    return GetContentAttribute(metadata, "Video", attributeName, defaultValue);

                case ContentType.TvShows:
                    return GetContentAttribute(metadata, "Video", attributeName, defaultValue);

                case ContentType.Music:
                    return GetContentAttribute(metadata, "Track", attributeName, defaultValue);
            }

            //fail-safe
            return defaultValue;
        }

        public static string GetContentAttribute(DataTable tableObject, string attributeName,
            string defaultValue = @"Unknown")
        {
            var attributeValue = defaultValue;
            try
            {
                //return the default value above if the data (table) is null
                if (tableObject == null)
                    return attributeValue;

                //the first row is the only information we will need
                var row = tableObject.Rows[0];

                //first, check if the specified attribute exists in the row data
                if (row[attributeName] != null)
                    if (!Equals(row[attributeName], string.Empty))
                        attributeValue = row[attributeName].ToString();
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "GetAttrError");
            }

            return attributeValue;
        }

        public static string GetContentAttribute(XmlDocument metadata, string tableName, string attributeName,
            string defaultValue = @"Unknown")
        {
            var attributeValue = defaultValue;
            try
            {
                var sections = new DataSet();
                sections.ReadXml(new XmlNodeReader(metadata));

                DataTable data = null;

                //check if the table we want is actually present
                if (sections.Tables.Contains(tableName))
                    data = sections.Tables[tableName];

                attributeValue = GetContentAttribute(data, attributeName, defaultValue);
            }
            catch (Exception ex)
            {
                LoggingHelpers.RecordException(ex.Message, "GetAttrError");
            }

            return attributeValue;
        }
    }
}