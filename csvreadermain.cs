using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class Main
{
	List<Dictionary<string, object>> data = CSVReader.read("mockdata");
	
	for(var i = 0; i < data.Count; i++) {
		print ( "min: " + data[i]["min"] + "\n" +
				"max: " + data[i]["max"] + "\n" +
				"updateInt: " + data[i]["updateInt"] + "\n" +
				"numGauges: " + data[i]["numGauges"] + "\n" +
				"initPattern: " + data[i]["initPattern"] + "\n" +
				"growthPattern: " + data[i]["growthPattern"] + "\n" );
	}
	
	return 0;
}