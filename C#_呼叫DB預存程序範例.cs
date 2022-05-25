private void InsertData3()
{
	string RequisitionID = "1111111";
	DataTable dt = new DataTable();
	string cs = "Data Source=10.1.0.25;Password=newtype;User ID=sa;Initial Catalog=BPMDEV";
	string qs = "select count(*) as Detail_Item from FM7T_AC02_Poject where RequisitionID=@RequisitionID";
	string qs2 = "select count(*) as Detail_Invoice from FM7T_AC02_Inrecords where RequisitionID=@RequisitionID";            
	SqlConnection conn = new SqlConnection(cs);
	conn.Open();

	SqlCommand cmd = new SqlCommand(qs, conn);
	cmd.Parameters.AddWithValue("RequisitionID", RequisitionID);
	SqlDataReader reader = cmd.ExecuteReader();
	reader.Read();
	int Detail_Item = Convert.ToInt32(reader["Detail_Item"]);
	reader.Close();

	SqlCommand cmd2 = new SqlCommand(qs2, conn);
	cmd2.Parameters.AddWithValue("RequisitionID", RequisitionID);
	SqlDataReader reader2 = cmd2.ExecuteReader();
	reader2.Read();
	int Detail_Invoice = Convert.ToInt32(reader2["Detail_Invoice"]);
	reader2.Close();

	string sqlstr = "";
	if (Detail_Item >= Detail_Invoice)
	{
		sqlstr = "SELECT a.RequisitionID, a.AccSubject, a.DeptID, a.SuNameUse, a.Quantity, a.UnitPrice,a.TotalUntaxed, a.SAP_Assignment, a.SAP_ClassText, a.RDProjectID, a.item_no, b.AutoCounter, b.RequisitionID, b.Sales, b.TotalUntaxed as Totaltaxed,b.InvoiceDate, b.InvoiceNumber, b.VATNumber, b.FormatType, a.item_no FROM FM7T_AC02_Poject AS a LEFT OUTER JOIN FM7T_AC02_Inrecords AS b ON a.RequisitionID = b.RequisitionID AND a.item_no = b.item_no where a.RequisitionID=@RequisitionID";
	}
	else if (Detail_Item < Detail_Invoice)
	{
		sqlstr = "SELECT a.RequisitionID,a.Sales,a.TotalUntaxed as Totaltaxed,a.InvoiceDate,a.InvoiceNumber,a.VATNumber,a.FormatType,a.item_no,b.AccSubject,b.DeptID, b.SuNameUse,b.Quantity, b.UnitPrice, b.TotalUntaxed AS TotalUntaxed, b.SAP_Assignment, b.SAP_ClassText, b.RDProjectID FROM FM7T_AC02_Inrecords AS a LEFT OUTER JOIN FM7T_AC02_Poject AS b ON a.RequisitionID = b.RequisitionID AND a.item_no = b.item_no where a.RequisitionID=@RequisitionID";
	}

	DataTable zfit002 = new DataTable();
	SqlCommand cmd3 = new SqlCommand(sqlstr, conn);
	cmd3.Parameters.AddWithValue("RequisitionID", RequisitionID);
	SqlDataReader reader3 = cmd3.ExecuteReader();
	zfit002.Load(reader3);


	qs = "select BillingNumber,PayCurrency from fm7t_ac02_m where RequisitionID=@RequisitionID";
	SqlCommand cmd4 = new SqlCommand(qs, conn);
	cmd4.Parameters.AddWithValue("RequisitionID", RequisitionID);
	SqlDataReader reader4 = cmd4.ExecuteReader();
	reader4.Read();
	string BillingNumber = reader4["BillingNumber"].ToString();
	string PayCurrency = reader4["PayCurrency"].ToString();
	reader4.Close();

	conn.Close();


	cs = ConfigurationManager.ConnectionStrings["cnnStr_AX-SAPDB"].ConnectionString;
	OracleConnection Oconn = new OracleConnection(cs);
	Oconn.Open();

	//呼叫Stored Procedure
	OracleCommand Ocmd = new OracleCommand("ZFIT_PROCEDURE", Oconn);
	//設定command type為StoredProcedure
	Ocmd.CommandType = CommandType.StoredProcedure;


	//Ocmd.Parameters.Add(new OracleParameter("ret", OracleDbType.Int32));
	//Ocmd.Parameters["ret"].Direction = ParameterDirection.ReturnValue;
	//加入參數no_in
	//Ocmd.Parameters.Add(new OracleParameter("NO_IN", OracleDbType.Int32));
	//參數方向in/out/in out
	//Ocmd.Parameters["NO_IN"].Direction = ParameterDirection.InputOutput;
	//參數值
	//Ocmd.Parameters["NO_IN"].Value = 1;

	for (int i = 0; i < zfit002.Rows.Count; i++)
	{

		//qs = "insert into sapsr3.ZFIT002 (MANDT,HKONT,KOSTL,TEXT1,MENGE,PRICE,AMT02,AMT03,AMT04,ZFBDT,ZUONR,XREF1,XREF2,AUFNR,ZNUM,ZITEM,WAERS,ZASIGM,ZGTXT,SGTXT) " +
		//         "values (:MANDT,:HKONT,:KOSTL,:TEXT1,:MENGE,:PRICE,:AMT02,:AMT03,:AMT04,:ZFBDT,:ZUONR,:XREF1,:XREF2,:AUFNR,:ZNUM,N'" + (i + 1).ToString() + "',:WAERS,:ZASIGM,:ZGTXT,:SGTXT)";

		//OracleCommand Ocmd = new OracleCommand(qs, Oconn);
		//OracleTransaction OTransaction = Oconn.BeginTransaction();
		//Ocmd.Transaction = OTransaction;

		Ocmd.Parameters.Add("MANDT", OracleDbType.NVarchar2).Value = "128";
		string AccSubject = "_";
		if (Convert.ToString(zfit002.Rows[i]["AccSubject"]) != "0" && Convert.ToString(zfit002.Rows[i]["AccSubject"]) != "")
			AccSubject = "000" + Convert.ToString(zfit002.Rows[i]["AccSubject"]).Trim().Substring(0, 7);
		Ocmd.Parameters.Add("HKONT", OracleDbType.NVarchar2).Value = AccSubject;                

		string DeptID = "_";
		if (Convert.ToString(zfit002.Rows[i]["AccSubject"]) != "5800900" && Convert.ToString(zfit002.Rows[i]["DeptID"]) != "")
			DeptID = Convert.ToString(zfit002.Rows[i]["DeptID"]).Substring(0, 7);
		Ocmd.Parameters.Add("KOSTL", OracleDbType.NVarchar2).Value = DeptID;

		string SuNameUse = "_";
		if (Convert.ToString(zfit002.Rows[i]["SuNameUse"]) != "")
		{
			SuNameUse = Convert.ToString(zfit002.Rows[i]["SuNameUse"]);
		}
		Ocmd.Parameters.Add("TEXT1", OracleDbType.NVarchar2).Value = SuNameUse;


		Double Quantity = 0;
		if (Convert.ToString(zfit002.Rows[i]["Quantity"]) != "")
			Quantity = Convert.ToDouble(zfit002.Rows[i]["Quantity"]);
		Ocmd.Parameters.Add("MENGE", OracleDbType.Double).Value = Quantity;


		Double UnitPrice = 0;
		if (Convert.ToString(zfit002.Rows[i]["UnitPrice"]) != "")
		{
			UnitPrice = Convert.ToDouble(zfit002.Rows[i]["UnitPrice"]);
			//if(Convert.ToString(zfit002.Rows[0]["PayCurrency"])=="TWD" || Convert.ToString(zfit002.Rows[0]["PayCurrency"]) == "JPY")
		}
		Ocmd.Parameters.Add("PRICE", OracleDbType.Double).Value = UnitPrice;


		Double Sales = 0;
		if (Convert.ToString(zfit002.Rows[i]["Sales"]) != "")
		{
			Sales = Convert.ToDouble(zfit002.Rows[i]["Sales"]);
			if (PayCurrency == "TWD" || PayCurrency == "JPY")
				Sales = Convert.ToDouble(zfit002.Rows[i]["Sales"]) / 100;
		}
		Ocmd.Parameters.Add("AMT02", OracleDbType.Double).Value = Sales;


		Double Totaltaxed = 0;
		if (Convert.ToString(zfit002.Rows[i]["Totaltaxed"]) != "")
		{
			Totaltaxed = Convert.ToDouble(zfit002.Rows[i]["Totaltaxed"]);
			if (PayCurrency == "TWD" || PayCurrency == "JPY")
				Totaltaxed = Convert.ToDouble(zfit002.Rows[i]["Totaltaxed"]) / 100;
		}
		Ocmd.Parameters.Add("AMT03", OracleDbType.Double).Value = Totaltaxed;


		Double TotalUntaxed = 0;
		if (Convert.ToString(zfit002.Rows[i]["TotalUntaxed"]) != "")
		{
			TotalUntaxed = Convert.ToDouble(zfit002.Rows[i]["TotalUntaxed"]);
			if (PayCurrency == "TWD" || PayCurrency == "JPY")
				TotalUntaxed = Convert.ToDouble(zfit002.Rows[i]["TotalUntaxed"]) / 100;
		}
		Ocmd.Parameters.Add("AMT04", OracleDbType.Double).Value = TotalUntaxed;


		string InvoiceDate = "00000000";
		if (Convert.ToString(zfit002.Rows[i]["InvoiceDate"]) != "")
			InvoiceDate = Convert.ToDateTime(zfit002.Rows[i]["InvoiceDate"]).ToString("yyyyMMdd");
		Ocmd.Parameters.Add("ZFBDT", OracleDbType.NVarchar2).Value = InvoiceDate;


		string InvoiceNumber = "_";
		if (Convert.ToString(zfit002.Rows[i]["InvoiceNumber"]) != "")
			InvoiceNumber = Convert.ToString(zfit002.Rows[i]["InvoiceNumber"]);
		Ocmd.Parameters.Add("ZUONR", OracleDbType.NVarchar2).Value = InvoiceNumber;


		string VATNumber = "_";
		if (Convert.ToString(zfit002.Rows[i]["VATNumber"]) != "")
			VATNumber = Convert.ToString(zfit002.Rows[i]["VATNumber"]).Replace("-", "");
		Ocmd.Parameters.Add("XREF1", OracleDbType.NVarchar2).Value = VATNumber;


		string FormatType = "_";
		if (Convert.ToString(zfit002.Rows[i]["FormatType"]) != "")
			FormatType = Convert.ToString(zfit002.Rows[i]["FormatType"]);
		Ocmd.Parameters.Add("XREF2", OracleDbType.NVarchar2).Value = FormatType;


		string RDProjectID = "_";
		if (Convert.ToString(zfit002.Rows[i]["RDProjectID"]) != "")
			RDProjectID = Convert.ToString(zfit002.Rows[i]["RDProjectID"]);
		Ocmd.Parameters.Add("AUFNR", OracleDbType.NVarchar2).Value = RDProjectID;


		string item_no = "";
		if (Convert.ToString(zfit002.Rows[i]["item_no"]) != "")
		{
			//item_no = Convert.ToString(zfit002.Rows[i]["item_no"]);
			item_no = (i + 1).ToString();
		}
		Ocmd.Parameters.Add("ZITEM", OracleDbType.NVarchar2).Value = item_no;


		string SAP_Assignment = "_";
		if (Convert.ToString(zfit002.Rows[i]["SAP_Assignment"]) != "")
			SAP_Assignment = Convert.ToString(zfit002.Rows[i]["SAP_Assignment"]);
		Ocmd.Parameters.Add("ZASIGM", OracleDbType.NVarchar2).Value = SAP_Assignment;


		string SAP_ClassText = "_";
		if (Convert.ToString(zfit002.Rows[i]["SAP_ClassText"]) != "")
			SAP_ClassText = Convert.ToString(zfit002.Rows[i]["SAP_ClassText"]);
		Ocmd.Parameters.Add("ZGTXT", OracleDbType.NVarchar2).Value = SAP_ClassText;


		Ocmd.Parameters.Add("ZNUM", OracleDbType.NVarchar2).Value = BillingNumber;
		Ocmd.Parameters.Add("WAERS", OracleDbType.NVarchar2).Value = PayCurrency;



		string Acc = "";
		if (Convert.ToString(zfit002.Rows[i]["AccSubject"]).Trim() != "")
		{
			Acc = Convert.ToString(zfit002.Rows[i]["AccSubject"]).Trim().Substring(0, 7);
		}

		if (Acc == "6151002" || Acc == "5800900" || Acc == "6152001" || Acc == "6152002" || Acc == "6152009")
		{
			Ocmd.Parameters.Add("SGTXT", OracleDbType.NVarchar2).Value = SAP_ClassText + "/" + SuNameUse;
			//Response.Write("SGTXT="+SAP_ClassText + "/" + SuNameUse + "<br>");
		}
		else
		{
			Ocmd.Parameters.Add("SGTXT", OracleDbType.NVarchar2).Value = SAP_ClassText;
			//Response.Write("SGTXT=" + SAP_ClassText + "<br>");
		}

		#region

		Ocmd.Parameters["MANDT"].Direction = ParameterDirection.InputOutput;                
		Ocmd.Parameters["HKONT"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["KOSTL"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["TEXT1"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["MENGE"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["PRICE"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["AMT02"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["AMT03"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["AMT04"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["ZFBDT"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["ZUONR"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["XREF1"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["XREF2"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["AUFNR"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["ZNUM"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["ZITEM"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["WAERS"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["ZASIGM"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["ZGTXT"].Direction = ParameterDirection.InputOutput;
		Ocmd.Parameters["SGTXT"].Direction = ParameterDirection.InputOutput;

		#endregion

		Response.Write("AccSubject=" + AccSubject + " <br>");
		Response.Write("DeptID=" + DeptID + " <br>");
		Response.Write("SuNameUse=" + SuNameUse + " <br>");
		Response.Write("Quantity=" + Quantity.ToString() + " <br>");
		Response.Write("UnitPrice=" + UnitPrice.ToString() + " <br>");
		Response.Write("Sales=" + Sales.ToString() + " <br>");
		Response.Write("Totaltaxed=" + Totaltaxed.ToString() + " <br>");
		Response.Write("TotalUntaxed=" + TotalUntaxed.ToString() + " <br>");
		Response.Write("InvoiceDate=" + InvoiceDate + " <br>");
		Response.Write("InvoiceNumber=" + InvoiceNumber + " <br>");
		Response.Write("VATNumber=" + VATNumber + " <br>");
		Response.Write("FormatType=" + FormatType + " <br>");
		Response.Write("RDProjectID=" + RDProjectID + " <br>");
		Response.Write("item_no:" + item_no + " <br>");
		Response.Write("SAP_Assignment=" + SAP_Assignment + " <br>");
		Response.Write("SAP_ClassText=" + SAP_ClassText + " <br>");
		Response.Write("BillingNumber:" + BillingNumber + " <br>");
		Response.Write("PayCurrency=" + PayCurrency + " <br>");
		Response.Write("============================<br>");
		//a = i;
		//Ocmd.ExecuteNonQuery();
		//OTransaction.Commit();
		Ocmd.Parameters.Clear();

	   

	}
	Oconn.Clone();

	Response.Write("<br>TRUE");
}