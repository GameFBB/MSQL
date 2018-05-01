using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSQL
{
    public class MSQLQuery
    {
        // PHP
        private string URL = "http://YOUR_WEB_SERVER/mysql_query.php";

        // Table
        private string _Table = "";

        // Select
        private string _Select = "";
        private List<string> _SelectList = new List<string>();

        // Where
        private string WhereCount = "";
        private string _Target = "";
        private List<string> _TargetList = new List<string>();
        private string _Operator = "";
        private List<string> _OperatorList = new List<string>();
        private string _Value = "";
        private List<object> _ValueList = new List<object>();

        // OrderBy
        private string _Field = "";
        private string _Order = "";

        // Limit
        private string _Limit = "";

        // Result
        private IList _Result;

        public IList Result
        {
            get { return _Result; }
            private set { _Result = value; }
        }

        // Count
        private int _Count;

        public int Count
        {
            get { return _Count; }
            private set { _Count = value; }
        }

        // Constructor
        public MSQLQuery(string Table)
        {
            _Table = Table;
        }

        // Select
        public MSQLQuery Select(params string[] Column)
        {
            for (int i = 0; i < Column.Length; ++i)
            {
                _SelectList.Add(Column[i]);
            }

            return this;
        }

        // Where
        public MSQLQuery Where(string Target, string Operator, object Value)
        {
            //bool -> TINYINT(1)
            if (Convert.ToString(Value) == "True")
            {
                Value = 1;
            }
            else if (Convert.ToString(Value) == "False")
            {
                Value = 0;
            }

            _TargetList.Add(Target);
            _OperatorList.Add(Operator);
            _ValueList.Add(Value);

            return this;
        }

        // OrderBy
        public MSQLQuery OrderBy(string Field, string Order)
        {
            _Order = Order;
            _Field = Field;

            return this;
        }

        // Limit
        public MSQLQuery Limit(int Limit)
        {
            _Limit = Convert.ToString(Limit);

            return this;
        }

        // Construct HTMLForm
        private WWWForm ConstructWWWForm(string _Method)
        {
            WWWForm Form = new WWWForm();

            Form.AddField("method", _Method);

            Form.AddField("table", _Table);

            _Select = Json.Serialize(_SelectList);
            Form.AddField("select", _Select);

            WhereCount = Convert.ToString(_TargetList.Count);
            Form.AddField("where", WhereCount);

            _Target = Json.Serialize(_TargetList);
            Form.AddField("target", _Target);

            _Operator = Json.Serialize(_OperatorList);
            Form.AddField("operator", _Operator);

            _Value = Json.Serialize(_ValueList);
            Form.AddField("value", _Value);

            Form.AddField("field", _Field);

            Form.AddField("order", _Order);

            if (_Limit == null)
            {
                Form.AddField("limit", "");
            }
            else
            {
                Form.AddField("limit", _Limit);
            }

            return Form;
        }

        // JSON -> IList
        private IList ConvertToIList(string JsonText)
        {
            if (JsonText == "[]")
            {
                _Result = null;
            }
            else
            {
                _Result = (IList)Json.Deserialize(JsonText);
            }
            return _Result;
        }

        // FindAsync
        public IEnumerator FindAsync()
        {
            // HTMLForm
            WWWForm Form = ConstructWWWForm("Find");

            // HTMLForm → PHP
            WWW www = new WWW(URL, Form);
            yield return www;

            if (www == null)
            {
                Debug.Log(www.text);
                yield break;
            }
            else
            {
                // JSON -> IList
                Result = ConvertToIList(www.text);
            }
        }

        // CountAsync
        public IEnumerator CountAsync()
        {
            // HTMLForm
            WWWForm Form = ConstructWWWForm("Count");

            // HTMLForm → PHP
            WWW www = new WWW(URL, Form);
            yield return _Result;

            try
            {
                Count = Convert.ToInt32(www.text);
            }
            catch (Exception)
            {
                Debug.Log(www.text);
                yield break;
            }
        }
    }
}