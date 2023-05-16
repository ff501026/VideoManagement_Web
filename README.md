# 影音借閱資訊系統 Web
## 說明
此系統是由ASP.NET MVC進行開發，前端使用Kendo、JQuery及AJAX，後端則使用C#及MS SQL，並將程式分為DAO、Service、Controller、Model及Common層。

由DAO負責進行DB的CRUD操作；Service再將DAO的內容包裝成服務提供給Controller，但由於此系統較為單純所以Service沒做太多處理；
最後Controller負責處理View的請求及做View與Service之間的資料處理；Model則是用來儲存資料模型；Common是存放共用的Method，例如:Decode或Log等。

除了上述五層外還有Test層用來進行簡易單元測試，包括測試刪除邏輯是否正確：已借閱及已借閱未領 -> 不能刪除此影音；可以借閱及不可借閱 ->可以刪除此影音 等。

### Layer架構圖
* DAO：進行DB的CRUD操作。
* Service：將DAO的內容包裝成服務。
* Controlelr：處理View的請求及做View與Service之間的資料處理。
* Model：資料模型。
* Common：共用Method。
* Test：單元測試。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/3e9d6b5c-0a70-4a1b-b13e-7df3da7500d2)
## 使用說明
使用前請先執行`DataBase/VideoManagement_Setup.sql`建立資料庫，並至`VideoManagement/Web.config`修改`DBConn`的連接字串。
## 功能介紹
### 首頁-查詢
* 首頁為查詢畫面，預設會帶出所有影音資訊及借閱狀態，點選影片名稱可以看明細資料。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/59755a1b-a579-4b60-a94d-216596e29b9f)
* 上方可輸入條件進行查詢。
* 點選清除可清除查詢條件。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/a06966b1-4036-42a8-8e90-870c4434fd12)
* 表格的標題可進行資料排序。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/c76817bb-d300-4781-93b9-4b5a8a14c0ab)

### 新增
* 點選首頁上的新增或最上排功能列的新增，即可進入新增畫面。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/8984cb78-3e55-4fa7-b9de-386b9fac775a)
* 新增時會先進行資料驗證，必填欄位及資料格式皆沒問題則可新增。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/c528d581-4978-4a71-a04c-0c16a2fbb601)
* 新增成功會顯示成功訊息，並清空輸入資料讓使用者進行下筆新增。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/21240eec-a193-4a39-96ad-25eca49edc8a)
* 新增成功後預設狀態為可以借出。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/b0634168-29d7-41da-9555-d70446a1be0b)

### 明細
* 點選首頁表格內的影片名稱可查看影片明細，且皆為ReadOnly
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/91034522-4f98-4d39-b0e6-0a0c8ad22f62)

### 編輯
* 點選首頁表格內的編輯按鈕，可對該筆資料進行修改。
* 借閱狀態若為可以借出/不可借出，則借閱人為ReadOnly
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/3073f726-b27f-460b-80ef-bb7710431801)
* 編輯前會進行資料驗證，必填欄位及資料格式皆沒問題則可修改。
* 點選還原可恢復原資料。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/5792f02d-c468-4a47-906d-8923300d35df)
* 編輯成功會顯示成功訊息，若有修改借閱人則會顯示新增借閱紀錄之訊息。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/e146b1bc-13ea-40c0-b8f0-0bba8f418902)

### 借閱紀錄
* 點選首頁表格內的借閱紀錄按鈕，可查看該筆資料的借閱紀錄。
* 表格的標題可進行資料排序。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/44181b11-42c6-48f0-8ea4-3a6b7f8f9eeb)

### 刪除
* 點選首頁表格內的刪除按鈕，可刪除該筆資料。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/fb2ca5f1-9b03-4e4a-a80f-3d1df7a2cb78)
* 刪除前會檢查影片是否被借出，若為借出狀態則不可刪除。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/417287a6-8784-4303-94ff-41b4b2dc9bef)
* 刪除成功會顯示成功訊息，並刪除該資料。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/891dd996-77e1-4ef1-8a7e-89e6cf30c1c9)
* 進入編輯頁面點選刪除按鈕，可刪除該筆資料。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/6e437c73-076b-4a91-a1ca-489eef7122ab)
* 刪除前會檢查影片是否被借出，若為借出狀態則不可刪除。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/72bbc625-ebfc-4445-a3d7-ce81d6b1db69)
* 刪除成功會顯示成功訊息，按下確認後會跳轉至首頁。
![image](https://github.com/ff501026/VideoManagement_Web/assets/103199969/5a765535-4027-44b4-a204-f3610b283567)

