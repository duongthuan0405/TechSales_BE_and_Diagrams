# Rules

Các hành động, kiểm tra, check, validate và node quyết định (đúng/sai, hợp lệ/không hợp lệ), được hợp nhất lại thành 1 node quyết định duy nhất, có đánh số, và tên node là một hành động

Các hành động yêu cầu người dùng xác nhận, thì bên hệ thống vẫn có 1 node action thể hiện Yêu cầu xác nhận, rồi trỏ lại về actor, có 1 node quyết định thể hiện rằng Xác nhận hành động (không cần tách ra node Xác nhân, rồi mới tới node quyết định là đúng/sai)
