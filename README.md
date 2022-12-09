# Document for API doc




| API                 | Method | URL param        | Body                                                                                                                                                                                                                                       | Note                                   |
|---------------------|--------|------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------------|
| api/manager/auth    | POST   | x                | username: string<br/>password: string                                                                                                                                                                                                      | Authenticate (xài cho tất cả user)     |
| api/user/{username} | GET    | username: string | x                                                                                                                                                                                                                                          | Lấy thông tin user từ username         |
| api/user/{username} | DELETE | username: string | x                                                                                                                                                                                                                                          | Xóa username ra khỏi db                |
| api/user/all        | GET    | x                | x                                                                                                                                                                                                                                          | Lấy tất cả thông tin user lưu trong db |
| api/user/create     | POST   | x                | idUser: string (truyền rỗng " ")<br/>userName: string<br/>password:string<br/>firstName: string<br/> lastName: string<br/>email: string<br/>phoneNumber:string<br/>gender: string (Male/Female)<br/>typeUser: int (1: student, 2: teacher) | Thêm user vào DB                       |

| api/user/{username} | PUT    | username: string | userName: string<br/>password:string<br/>firstName: string<br/> lastName: string<br/>email: string<br/>phoneNumber:string<br/>gender: string (Male/Female)<br/>typeUser: int (1: student, 2: teacher)                                      | Sửa thông tin user dựa trên username   |
