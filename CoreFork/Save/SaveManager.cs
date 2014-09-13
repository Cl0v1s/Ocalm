using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Text;
using System.Globalization;

namespace CoreFork
{
    public class SaveManager
    {

        public static List<KeyValuePair<string, Saver>> list = new List<KeyValuePair<string, Saver>>();
        public static bool toReset = false;

        /// <summary>
        /// Créer une nouvelle sauvegarde associée au nom passé en paramètre
        /// </summary>
        /// <param name="par1name">Nom de la sauvegarde</param>
        /// <returns></returns>
        public static bool createSaver(string par1name)
        {
            foreach (KeyValuePair<string, Saver> en in list)
            {
                if (en.Key == par1name)
                    return false;
            }
            list.Add(new KeyValuePair<string, Saver>(par1name, new Saver(par1name)));
            return true;
        }

        /// <summary>
        /// Créer une nouvelle sauvegarde associée au nom passé en paramètre
        /// </summary>
        /// <param name="par1name">Nom de la sauvegarde</param>
        /// <returns></returns>
        public static bool createSaver(string par1name, Saver par2saver)
        {
            if (par2saver.name == par1name)
            {
                foreach (KeyValuePair<string, Saver> en in list)
                {
                    if (en.Key == par1name)
                        return false;
                }
                list.Add(new KeyValuePair<string, Saver>(par1name, par2saver));
                return true;
            }
            else
            {
                throw new Exception("Le nom de l'unité de sauvegarde et le nom de la clef doivent correspondre.");
            }
        }

        /// <summary>
        /// remplace une unité de sauvegarde associée au nom passé en paramètre
        /// </summary>
        /// <param name="par1name">Nom de la sauvegarde</param>
        /// <returns></returns>
        public static bool replaceSaver(string par1name,Saver par2saver)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Key == par1name)
                {
                    list[i] = new KeyValuePair<string, Saver>(par1name, par2saver);
                    return true;
                }
            }
			createSaver (par1name, par2saver);
            return false;
        }

        /// <summary>
        /// Récupère une unité de sauvegarde en fonction de soon nom
        /// </summary>
        /// <param name="par1name"></param>
        /// <returns></returns>
        public static Saver getSaver(string par1name)
        {
            foreach (KeyValuePair<string, Saver> en in list)
            {
                if (en.Key == par1name)
                    return en.Value;
            }
            return null;
        }

        /// <summary>
        /// Sauvegarde toutes les unités de sauvegarde en mémoire
        /// </summary>
        public static void save()
        {
            if (!toReset)
            {
                createSaver("SaverList");
                Saver l = getSaver("SaverList");
                int o = 0;
                foreach (KeyValuePair<string, Saver> en in list)
                {
                    l.addData(o.ToString(), en.Key);
                    o += 1;
                }
                foreach (KeyValuePair<string, Saver> en in list)
                {
                    en.Value.save();
                }
            }
        }

        /// <summary>
        /// Charge toutes les unités de sauvegarde en fonction de celle répertoriés dans l'unité de sauvegarde SaverList
        /// </summary>
        public static bool load()
        {
            Saver l = new Saver("SaverList");
            l = l.load();
            if (l == null)
                return false;
            for (int i = 0; i < l.count(); i++)
            {
                string n = l.getDataAsString(i.ToString());
                Saver s = new Saver(n);

                s = s.load();
                createSaver(n, s);
            }
            return true;
        }
        
        /// <summary>
        /// Efface la liste lien avec tout les fichiers de sauvegarde présents dans la mémoire
        /// Efface la mémoire.
        /// </summary>
        public static void reset()
        {
            Saver m = getSaver("SaverList");
            if (m == null)
            {
                m = new Saver("SaverList").load();
                if (m == null)
                    return;
            }
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                for (int i = 0; i < m.count(); i++)
                {
                    string n = m.getDataAsString(i.ToString());

                    if (storage.FileExists(n + ".sav"))
                        storage.DeleteFile(n + ".sav");
                }
                if (storage.FileExists("SaverList" + ".sav"))
                    storage.DeleteFile("SaverList" + ".sav");
                list = new List<KeyValuePair<string, Saver>>();
            }
            SaveManager.toReset = true;
        }
    }
}
