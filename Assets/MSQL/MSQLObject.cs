using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSQL
{
    public class MSQLObject
    {
        //PHP
        private string URL = "http://YOUR_WEB_SERVER/msql_object.php";

        //Method
        private string _Method = "";

        //Table
        private string _Table = "";

        //Add
        private string _SaveField = "";
        private List<string> _SaveFieldList = new List<string>();
        private string _SaveValue = "";
        private List<object> _SaveValueList = new List<object>();

        //Update
        private string _Target = "";
        private object _TargetValue = "";

        //Constructor : Insert, Find
        public MSQLObject(string Table)
        {
            _Table = Table;
        }

        //Constructor : Update, Delete
        public MSQLObject(string Table, string Target, object TargetValue)
        {
            //bool -> TINYINT(1)
            if (Convert.ToString(TargetValue) == "True")
            {
                _TargetValue = 1;
            }
            else if (Convert.ToString(TargetValue) == "False")
            {
                _TargetValue = 0;
            }

            _Table = Table;
            _Target = Target;
            _TargetValue = TargetValue;

        }

        //Add
        public MSQLObject Add(string SaveField, object SaveValue)
        {
            //bool -> TINYINT(1)
            if (Convert.ToString(SaveValue) == "True")
            {
                SaveValue = 1;
            }
            else if (Convert.ToString(SaveValue) == "False")
            {
                SaveValue = 0;
            }

            _SaveFieldList.Add(SaveField);
            _SaveValueList.Add(SaveValue);

            return this;
        }

        //Save
        public IEnumerator SaveAsync()
        {
            //HTMLForm
            if (_Target != "")
            {
                _Method = "Update";
            }
            else
            {
                _Method = "Insert";
            }
            WWWForm Form = ConstructWWWForm(_Method);

            //HTMLForm → PHP
            WWW _Send = new WWW(URL, Form);
            yield return _Send;
            Debug.Log(_Send.text);
        }

        //Delete
        public IEnumerator DeleteAsync()
        {
            //HTMLForm
            _Method = "Delete";
            WWWForm Form = ConstructWWWForm(_Method);

            //HtmlForm → PHP
            WWW _Send = new WWW(URL, Form);
            yield return _Send;
            Debug.Log(_Send.text);
        }

        //Construct HTMLForm
        private WWWForm ConstructWWWForm(string Method)
        {
            WWWForm Form = new WWWForm();

            Form.AddField("method", Method);

            Form.AddField("table", _Table);

            _SaveField = Json.Serialize(_SaveFieldList);
            Form.AddField("field", _SaveField);

            _SaveValue = Json.Serialize(_SaveValueList);
            Form.AddField("value", _SaveValue);

            Form.AddField("target", _Target);
            Form.AddField("targetValue", Convert.ToString(_TargetValue));

            return Form;
        }
    }
}