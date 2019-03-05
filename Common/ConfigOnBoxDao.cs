using iBoxDB.LocalServer;
using MovistarPlus.Common.Dto;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MovistarPlus.Common
{
	public class ConfigOnBoxDao : IDisposable
	{
		const string TABLENAME = "Config";

		private readonly ILog logger;
		private readonly DB db;
		private readonly DB.AutoBox auto;
		private readonly IBox box;

		public ConfigOnBoxDao(string baseFolder = null, ILog logger = null)
		{
			this.logger = logger;
			if (!string.IsNullOrWhiteSpace(baseFolder))
			{
				if (Process.GetCurrentProcess().IsAsp())
					baseFolder = System.Web.HttpContext.Current.Server.MapPath(baseFolder);
				DB.Root(baseFolder);
				this.logger?.Message($"iBox creado apuntando a carpeta {baseFolder}");
			}
			else
			{
				this.logger?.Message($"iBox creado apuntando a carpeta de ejecución");
			}
			this.db = new DB();
			//db.GetConfig().EnsureTable<BoxConfigDto>(TABLENAME, "Id");
			this.auto = db.Open();
			this.box = auto.Cube();
		}
		public string GetContent()
		{
			var BoxConfigDtoList = this.box.Select<BoxConfigDto>($"from {TABLENAME}").ToList();
			this.logger?.Message($"Contenido del iBox:");
			BoxConfigDtoList.ForEach(x =>
			{
				this.logger?.Message($"Id: {x.Id}");
				this.logger?.Message($"Content: {x.Content}");
			});
			if (BoxConfigDtoList.Count > 0)
				return BoxConfigDtoList[0].Content;
			else
				return null;
		}
		public void DeleteAll()
		{
			var BoxConfigDtoList = this.box.Select<BoxConfigDto>($"from {TABLENAME}").ToList();
			BoxConfigDtoList.ForEach(x =>
			{
				this.auto.Delete(TABLENAME, x.Id);
			});
			this.logger?.Message($"Borrados {BoxConfigDtoList.Count} elementos");
			this.box.Commit().Assert();
		}
		public void AddUpdateFromFile(string fileName)
		{
			if (!File.Exists(fileName))
				throw new ArgumentException($"No existe fichero {fileName}");

			string contentJson = File.ReadAllText(fileName);
			var BoxConfigDto = new BoxConfigDto() { Id = 0, Content = contentJson };
			if (this.auto.Get<BoxConfigDto>(TABLENAME, 0L) == null)
				this.box[TABLENAME].Insert<BoxConfigDto>(BoxConfigDto);
			else
				this.box[TABLENAME].Update<BoxConfigDto>(BoxConfigDto);
			this.logger?.Message($"Fichero insertado con Id: {BoxConfigDto.Id}");
			this.box.Commit().Assert();
		}
		public void AddUpdateFromString(string contentJson)
		{			
			var BoxConfigDto = new BoxConfigDto() { Id = 0, Content = contentJson };
			if (this.auto.Get<BoxConfigDto>(TABLENAME, 0L) == null)
				this.box[TABLENAME].Insert<BoxConfigDto>(BoxConfigDto);
			else
				this.box[TABLENAME].Update<BoxConfigDto>(BoxConfigDto);
			this.logger?.Message($"Fichero insertado con Id: {BoxConfigDto.Id}");
			this.box.Commit().Assert();
		}
		public void Dispose()
		{
			this.db.Dispose();
		}
		public interface ILog
		{
			void Message(string message);
		}
	}
}
