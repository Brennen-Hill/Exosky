using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class CSVParser
{
    //public TextAsset csvFile; // Assign your CSV file in the Inspector

    public static void Parse(TextAsset csvFile, int start_col, int end_col, Action<float[]> lambda)
    {
        StringReader reader = new StringReader(csvFile.text);

        bool headerSkipped = false;

        while (reader.Peek() != -1) {
            //Get next line
            string line = reader.ReadLine();

            //Skip header
            if (!headerSkipped) {
                headerSkipped = true; // Skip the header line
                continue;
            }

            // Trim whitespace and check if the line is empty
            line = line.Trim();
            if (string.IsNullOrEmpty(line)) 
            {
                Debug.Log("skipped empty line");
                continue; // Skip empty lines
            }

            string[] str_values = line.Split(',');
            float[] values = new float[str_values.Length];

            try {
                for(int i = start_col; i <= end_col; i ++) {
//                    Debug.Log("INCOMING:");
//                    Debug.Log(str_values[i]);
                    values[i] = float.Parse(str_values[i].Trim());
                }
                lambda(values);
            } catch (FormatException e) {
//                Debug.LogError($"Error parsing line: {line}. Exception: {e.Message}");
            }
        }
    }
}