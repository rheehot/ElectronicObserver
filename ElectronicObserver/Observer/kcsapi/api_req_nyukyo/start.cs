﻿using Codeplex.Data;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Observer.kcsapi.api_req_nyukyo {

	public class start : APIBase {


		public override void OnRequestReceived( Dictionary<string, string> data ) {

			KCDatabase db = KCDatabase.Instance;

			DockData dock = db.Docks[int.Parse( data["api_ndock_id"] )];

			int shipID = int.Parse( data["api_ship_id"] );
			ShipData ship = db.Ships[shipID];

			if ( data["api_highspeed"] == "1" ) {

				ship.Repair();
				db.Material.InstantRepair--;

			} else if ( ship.RepairTime <= 60 ) {

				ship.Repair();

			} else {

				dock.State = 1;
				dock.ShipID = shipID;
				dock.CompletionTime = DateTime.Now.AddSeconds( ship.RepairTime );

			}

			db.Material.Fuel -= ship.RepairFuel;
			db.Material.Steel -= ship.RepairSteel;
				
			
			base.OnRequestReceived( data );
		}

		public override string APIName {
			get { return "api_req_nyukyo/start"; }
		}
	}

}