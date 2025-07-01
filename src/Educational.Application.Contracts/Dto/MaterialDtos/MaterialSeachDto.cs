using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educational.Dto.MaterialDtos
{
	public class MaterialSeachDto:Seach
	{
		public bool MaterialStatus { get; set; }

		public string MaterialName { get; set; }
	}
}
