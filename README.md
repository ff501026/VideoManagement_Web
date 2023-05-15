# 影音借閱資訊系統 Web
## 說明
此系統是由ASP.NET MVC進行開發，前端使用Kendo、JQuery及AJAX，後端則使用C#及MS SQL，並將程式分為DAO、Service、Controller、Model及Common層，由DAO負責進行DB的CRUD操作；
Service再將DAO的內容包裝成服務提供給Controller，但由於此系統較為單純所以Service沒做太多處理；最後Controller負責處理View的請求及做View與Service之間的資料處理；
Model則是用來儲存資料模型；Common是存放共用的Method，例如:Decode或Log等。
除了上述五層外還有Test層用來進行簡易單元測試，包括測試刪除邏輯是否正確：已借閱及已借閱未領 -> 不能刪除此影音；可以借閱及不可借閱 ->可以刪除此影音 等。

### Layer架構圖
* DAO：進行DB的CRUD操作。
* Service：將DAO的內容包裝成服務。
* Controlelr：處理View的請求及做View與Service之間的資料處理。
* Model：資料模型。
* Common：共用Method。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/3e9d6b5c-0a70-4a1b-b13e-7df3da7500d2)
## 功能說明
* 首頁為查詢畫面，預設會帶出所有影音資訊及借閱狀態，點選影片名稱可以看明細資料。
* 點選首頁上的新增或最上排頁簽的新增可進入新增畫面，新增時會先進行資料驗證，必填欄位及資料格式皆沒問題則可新增。
