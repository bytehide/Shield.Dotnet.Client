﻿using System;

namespace Shield.Client.Fr.Models.API.Protections {
	public class ProtectionLogDTO {
		public int User { get; set; }
		public int Prepare { get; set; } //Es el id de prepare
		public string Key { get; set; }
		public DateTime Date { get; set; }
		public string Output { get; set; }

	}
}
