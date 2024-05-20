[Câu hỏi chỉ cần trả lời lý thuyết, không cần code] - nếu bạn phải monitor performance của ứng dụng xổ số, bạn sẽ track những metrics gì?
Để tối ưu performance thì cần track những metrics sau:
01 Stored procedures:
	.Kiểm tra đã các lệnh select đã có Nolock chưa
	.Tối ưu các câu select bằng cách danh91 Index
	.Kiểm tra các điều kiện Jion đã đúng theo quy trình thứ tự cần lấy từ bảng A đến bảng B hay không
	.Bỏ các điều kiện where không cần thiết và hạn chết dùng Like
02 Code:
	.Review code
	.Bỏ các biến thừa không sử dụng
	.Tránh việc 1 chức năng mà xử lý nhiều nhiệp vụ cùng lúc (nên chia nhỏ ra)
	
XIN CẢM ƠN CÁC ANH CHỊ ĐÃ DÀNH THỜI GIAN ĐÃ XEM BÀI KHẢO HẠCH DEV
