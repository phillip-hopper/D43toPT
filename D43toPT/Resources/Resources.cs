using D43toPT.Door43;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace D43toPT.Resources
{
	class Resources
	{
		public static Chunks GetChunksV3(string bookID)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = string.Format("{0}.Resources.chunks_v3.{1}.json", assembly.GetName().Name, bookID.ToLower());

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				string json = reader.ReadToEnd();
				dynamic array = JsonConvert.DeserializeObject(json);
				return new Chunks(array);
			}
		}
	}


}
