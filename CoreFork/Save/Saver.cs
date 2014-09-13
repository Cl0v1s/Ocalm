using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;
using System.Globalization;

namespace CoreFork
{
    public class Saver
    {
        public string name;
        public List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Créer une nouvelle unité de sauvegarde
        /// </summary>
        /// <param name="par1name"></param>
        public Saver(string par1name)
        {
            name = par1name;
        }

        /// <summary>
        /// Ajoute une donnée à sauvegarder
        /// </summary>
        /// <param name="par1key">Nom de la clef renvoyant à la donnée</param>
        /// <param name="par2string">Donnée sous le format standard du string</param>
        /// <returns></returns>
        public bool addData(string par1key, object par2)
        {
            foreach (KeyValuePair<string,string> en in data)
            {
                if (en.Key == par1key)
                    return false;
            }
            data.Add(new KeyValuePair<string, string>(par1key, par2.ToString()));
            return true;
        }

        /// <summary>
        /// Remplace une donnée en fonction de sa clef
        /// </summary>
        /// <param name="par1key"></param>
        /// <param name="par2"></param>
        public bool replaceData(string par1key, object par2)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Key == par1key)
                {
                    data[i] = new KeyValuePair<string, string>(par1key, par2.ToString());
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Retourne la valeur associée à la clef passée en paramètre
        /// </summary>
        /// <param name="par1key"></param>
        /// <returns></returns>
        public int getDataAsInt(string par1key)
        {
            foreach (KeyValuePair<string, string> en in data)
            {
                if (en.Key == par1key)
                    return int.Parse(en.Value);
            }
            return -987654321;
        }

        /// <summary>
        /// Retourne la valeur associée à la clef passée en paramètre
        /// </summary>
        /// <param name="par1key"></param>
        /// <returns></returns>
        public float getDataAsFloat(string par1key)
        {
            foreach (KeyValuePair<string, string> en in data)
            {
                if (en.Key == par1key)
                    return float.Parse(en.Value);
            }
            return -987654321F;
        }

        /// <summary>
        /// Retourne la valeur associée à la clef passée en paramètre
        /// </summary>
        /// <param name="par1key"></param>
        /// <returns></returns>
        public double getDataAsDouble(string par1key)
        {
            foreach (KeyValuePair<string, string> en in data)
            {
                if (en.Key == par1key)
                    return double.Parse(en.Value);
            }
            return -987654321D;
        }

        /// <summary>
        /// Retourne la valleur associée à la clef passé en paramètre
        /// </summary>
        /// <param name="par1key"></param>
        /// <returns></returns>
        public string getDataAsString(string par1key)
        {
            foreach (KeyValuePair<string, string> en in data)
            {
                if (en.Key == par1key)
                    return en.Value;
            }
            return null;
        }

        /// <summary>
        /// Retourne la valeur associée à la clef passée en paramètre sous forme de booléan
        /// </summary>
        /// <param name="par1key"></param>
        /// <returns></returns>
        public bool getDataAsBool(string par1key)
        {
            foreach (KeyValuePair<string, string> en in data)
            {
                if (en.Key == par1key)
                {
                    if (en.Value == "True" || en.Value == "true")
                        return true;
                    else if (en.Value == "False" || en.Value == "false")
                        return false;

                }
            }
            throw new Exception("Clef de donnée non trouvée");
        }

        /// <summary>
        /// Charge les données sauvegardées en mémoire dans le fichier possédant le nom du saver
        /// </summary>
        /// <returns></returns>
        public Saver load()
        {
            string par1name = name;
			IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
            if (storage.FileExists(par1name+".sav"))
            {
                try
                {
                    Stream stream = storage.OpenFile(par1name+".sav", FileMode.Open);
                    byte[] data = new Byte[1024];
                    stream.Read(data, 0, data.Length);
                    stream.Close();
                    string s = Encoding.UTF8.GetString(data, 0, data.Length);
                    string[] dat = s.Split("!".ToCharArray());
                    Saver saver = new Saver(par1name);
                    for (int i = 0; i < dat.Length-1; i++)
                    {
                        if (dat[i].Length > 0)
                        {
                                List<string> da = new List<string>(dat[i].Split("/".ToCharArray()));
                                string key = da[0];
                                string value = da[1];
                                saver.addData(key, value);
                        }
                    }
                    storage.Dispose();
                    return saver;
                }
                catch (FormatException)
                {
                    storage.Dispose();
                    return null;
                }
            }
            storage.Dispose();
            return null;
        }


        /// <summary>
        /// Ecrit les données de cette unité de sauvegarde dans la mémoire de l'appareil
        /// </summary>
        public void save()
        {
			using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain())
            {
                if (storage.FileExists(name+".sav"))
                    storage.DeleteFile(name+".sav");
                Stream stream = storage.CreateFile(name+".sav");
                string dat="";
                foreach (KeyValuePair<string, string> en in data)
                {
                    dat = dat + en.Key + "/" + en.Value + "!";
                }
                byte[] retval = new byte[dat.Length];
                for (int ix = 0; ix < dat.Length; ++ix)
                {
                    char ch = dat[ix];
                    if (ch <= 0x7f) retval[ix] = (byte)ch;
                    else retval[ix] = (byte)'?';
                }
                stream.Write(retval, 0, retval.Length);
                stream.Close();

                storage.Dispose();
            }
        }

        /// <summary>
        /// Retourne le nombre de clef dans cette unité de sauvegarde
        /// </summary>
        /// <returns></returns>
        public int count()
        {
            return data.Count;
        }


    }
}
