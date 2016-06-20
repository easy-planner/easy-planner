/////////////////////////////////////////////////////////////
// Class responsible : Alexandre
// Last update : 20.06.2016
// Sprint 8
// Story(ies) : Create finalized documentation
/////////////////////////////////////////////////////////////

using System;
using System.Configuration;
using System.Runtime.Serialization;

namespace EasyPlanner
{
    [Serializable]
    internal class NoDataBaseUsableException : Exception
    {
        public NoDataBaseUsableException() : base("Echec de connection à la base de donnée !\nLes configurations suivantes ont été testées :\n\n1) " + NoDataBaseUsableException.connectionDetails("bd_easyplannerEntities") +
            "\n\n2) " + NoDataBaseUsableException.connectionDetails("bd_easyplannerEntitiesFB"))
        {
        }

        static string connectionDetails(string connectionString)
        {
            string total = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            int start = total.IndexOf('"');
            int end = total.IndexOf('"', start + 1);

            return total.Substring(start+1, end - start - 1).Replace(';','\n');
        }
    }
}