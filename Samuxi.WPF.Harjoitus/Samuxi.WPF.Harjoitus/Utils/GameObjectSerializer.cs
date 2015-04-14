using System.IO;
using System.Xml.Serialization;
using Samuxi.WPF.Harjoitus.Model;

namespace Samuxi.WPF.Harjoitus.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class GameObjectSerializer
    {
        /// <summary>
        /// Writes the specified filename.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">The filename.</param>
        /// <param name="serializableObject">The serializable object.</param>
        public static void Write<T>(string filename, T serializableObject)  where T : new()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new[] { typeof(System.Windows.Media.MatrixTransform), typeof(BreakthroughGame), typeof(CheckerGame) });

            using (StreamWriter writer = new StreamWriter(filename))
            {
                xmlSerializer.Serialize(writer, serializableObject);
            }
        }

        /// <summary>
        /// Reads the specified filename.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static T Read<T>(string filename) where T : new()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new[] { typeof(System.Windows.Media.MatrixTransform), typeof(BreakthroughGame), typeof(CheckerGame) });

            using (StreamReader reader = new StreamReader(filename))
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
            
        }
    }
}
