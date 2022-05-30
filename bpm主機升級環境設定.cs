1.安裝Oracle clinet (32/64 bit)
	#修改tnsname.ora
	#修改regedit=>HKEY_LOCAL_MACHINE=>SOFTWARE=>ORACLE=>NLS_LANG
	#修改環境變數
		a.Path:增加client/、clinet/bin
		b.增加ORACLE_HOME: D:\app\tkyrnj_bpm\product\11.2.0\client_3
		c.增加TMS_ADMIN: D:\app\tkyrnj_bpm\product\11.2.0\client_3\network\admin
	
2.確定AutoWeb的DB都能正常連線
	#輸入料號帶出品名

3.IIS的ASP網頁設定
	#IIS=>應用程式集區=>選取應用程式集區=>右鍵"進階設定"=>啟用32位元應用程式"true"
	#站台=>選取網站=>功能檢視=>ASP=>啟用上層路徑"true"=>套用

4.MSSQL
	#排程作業、Intergration Service設定:
		1.登入Intergration Service=>存放的封裝=>file system=>選擇封裝按右鍵"匯出封裝"
		2.copy封裝到新server=>登入新DB的Intergration Service=>file system=>右鍵"匯入封裝"
		3.在舊DB的作業=>選擇作業右鍵"編寫作業的指令碼為"=>CREATE=>新增查詢編輯器視窗=>copy SQL
		4.在新DB新開查詢=>貼上SQL=>適當修改@command(server名稱、加上connection的Password等等)=>執行SQL
	#防火牆1433 port設定
	
5.安裝VS、SSIS

6.工作排程器
7.IP切換

8.開啟Windows驗證:
	#開啟伺服器管理員=>新增功能(windows驗證)
	#在IIS開啟ADV_Verify.aspx=>驗證=>啟用Windows驗證
	#D:\NTWEB\AutoWeb3\Database\Project\BPM\FlowADSSO\connection的AXBPM.xdbc.xmf裡面的連線字串修改
	
9.SSL憑證更新:
	#開啟MMC=>"檔案"=>新增或移除嵌入式管理單元=>新增憑證=>選"電腦帳戶"=>"本機電腦"=>確定
	#左邊選單=>個人=>憑證=>右鍵匯入=>匯入新的憑證
	#對匯入的新憑證=>右鍵匯出=>選匯出為.cer或.pfx
	#開啟IIS，選擇最上層server=>伺服器憑證
		#若匯出.cer=>建立憑證要求=>輸入相關資訊=>選擇「Microsoft RSA SChannel Cryptographic Provider」，位元長度「2048」=>輸入儲存檔名
			=>完成憑證要求=>選取剛剛匯出的cer=>繫結443 port
		#若匯出.pfx=>匯入=>匯入剛剛產生的pfx=>繫結443 port
	
	