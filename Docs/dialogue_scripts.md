# 📞 Script Câu Thoại Phone Calls — TiemTraGenZ

> **Nhân vật chính:** Minh — chủ tiệm trà (người chơi điều khiển)
> **Brand Voice Minh:** Bình tĩnh, ngại nói nhiều, hay trả lời ngắn nhưng thật lòng

---

## 🟡 MẸ — Brand Voice: *Ấm áp · Lo lắng · Tự hào*

> Tên hiển thị: **"Mẹ"** | Nhân vật bạn bè đổi tên thành **"Hùng"** (xem phần Bạn Bè)

---

### `CALL_Mom_Day1` — Ngày 1 · Giới thiệu

**Trigger:** `currentDay == 1`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Mẹ | Con ơi, mẹ gọi xem con dọn chỗ ổn chưa. Hôm nay là ngày đầu tiên tiệm mở mà, mẹ hồi hộp lắm. |
| 2 | Minh | Dạ ổn rồi mẹ, con dọn từ sáng sớm rồi. Mẹ đừng lo. |
| 3 | Mẹ | Mẹ nhớ hồi bà ngoại còn sống, bà hay nói: muốn giữ nghề trà thì phải giữ cái tâm trước. Con nhớ không? |
| 4 | Minh | Dạ con nhớ. Con sẽ cố giữ đúng cái hồn của tiệm như bà dạy. |
| 5 | Mẹ | Thôi con lo mở hàng đi, đừng để khách đứng chờ. Mẹ ghé thăm con cuối tuần nha. Cố lên! |
| 6 | Minh | Dạ, con chờ mẹ. Mẹ đi đường cẩn thận nha. |

---

### `CALL_Mom_Day7` — Ngày 7 · Hỏi thăm tuần đầu

**Trigger:** `currentDay == 7`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Mẹ | Alo con, tuần đầu sao rồi? Có khách ghé không hay ế quá? |
| 2 | Minh | Có khách mẹ ơi, ít thôi nhưng mà vui. Mấy người hàng xóm hay ghé buổi sáng. |
| 3 | Mẹ | Nhớ ăn cơm đúng bữa nha con, đừng có mải buôn bán mà bỏ bữa. Mẹ lo cái vụ đó lắm. |
| 4 | Minh | Con ăn đủ rồi mẹ, không có bỏ bữa đâu. |
| 5 | Mẹ | À mà con có chào hàng xóm xung quanh chưa? Người ta ở gần mình, cư xử cho đẹp vào. Sau này mới nhờ được. |
| 6 | Minh | Rồi mẹ, con hay mang trà biếu mấy nhà gần đó. Người ta có vẻ thích. |

---

### `CALL_Mom_Day30_LowCapital` — Ngày 30 · Vốn thấp (capital < 2000)

**Trigger:** `currentDay == 30 && capital < 2000`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Mẹ | Con ơi... mẹ thấy con dạo này trầm lắm, có chuyện gì không? |
| 2 | Minh | Không có gì mẹ ơi... chỉ là buôn bán hơi chậm thôi. |
| 3 | Mẹ | Mẹ biết buôn bán khó lắm. Hồi đó bà ngoại cũng vậy, có tháng ế đến mức không đủ tiền mua trà. |
| 4 | Minh | Dạ... con biết. Con đang cố tìm cách cải thiện. |
| 5 | Mẹ | Nếu cần thì nói mẹ biết nha. Mẹ để dành ít tiền, không nhiều nhưng cũng đỡ. Con đừng ngại. |
| 6 | Minh | Thôi không cần đâu mẹ. Mẹ giữ lấy mà dùng. Con tự lo được. |
| 7 | Mẹ | Nhưng mà... đừng vay ngoài nha con. Mẹ sợ lắm mấy chỗ đó. |
| 8 | Minh | *(im lặng một chút)* Dạ mẹ. Con nhớ rồi. |

---

### `CALL_Mom_Day30_HighCapital` — Ngày 30 · Vốn ổn (capital >= 2000)

**Trigger:** `currentDay == 30 && capital >= 2000`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Mẹ | Alo con! Mẹ nghe hàng xóm kể tiệm con đông khách lắm, có đúng không? |
| 2 | Minh | Dạ cũng ổn mẹ ơi, tháng này khá hơn tháng trước nhiều rồi. |
| 3 | Mẹ | Mẹ vui lắm con ơi. Bà ngoại mà còn sống chắc bà cũng tự hào. |
| 4 | Minh | Con cũng nghĩ vậy. Con cố gắng giữ cách pha trà theo kiểu bà dạy mẹ đó. |
| 5 | Mẹ | Nhớ đừng có ham tiền quá mà quên người xung quanh nha. Giàu mà cô đơn thì buồn lắm. |
| 6 | Minh | Dạ mẹ. Con hiểu rồi. |

---

### `CALL_Mom_LowRelation` — Ngày 60 · Tình làng thấp (neighborRelation < 30)

**Trigger:** `currentDay == 60 && neighborRelation < 30`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Mẹ | Con ơi, mẹ có nghe chú Tám nói mấy nhà gần tiệm con hơi... khó chịu gì đó. |
| 2 | Minh | Ui... thật mẹ? Con không biết. Chắc vì dạo này con bận quá không để ý. |
| 3 | Mẹ | Mẹ không biết chuyện gì, nhưng mà hàng xóm quan trọng lắm con à. Người ta có thể giúp mình lúc khó. |
| 4 | Minh | Dạ con biết. Để con ghé thăm mấy nhà đó. |
| 5 | Mẹ | Con thử ghé thăm họ, biếu gói trà hay cái gì đó. Không tốn nhiều nhưng người ta nhớ hoài. |
| 6 | Minh | Ý hay đó mẹ. Con làm vậy được. Cảm ơn mẹ nhắc. |

---

### `CALL_Mom_Final` — Ngày 89 · Cuộc gọi cuối

**Trigger:** `currentDay == 89`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Mẹ | Con ơi, không biết sao hôm nay mẹ cứ nhớ con mãi. Gọi cho yên tâm. |
| 2 | Minh | Con đây mẹ. Con đang ổn. |
| 3 | Mẹ | Ba tháng rồi đó, con biết không? Mẹ nhớ ngày con khóa cửa nhà, xách đồ đi với cái ba lô cũ. |
| 4 | Minh | Con cũng nhớ ngày đó mẹ. Hồi đó con sợ lắm, nhưng không dám nói. |
| 5 | Mẹ | Dù kết quả ra sao, con đã dám thử. Mà với mẹ, vậy là đủ rồi. |
| 6 | Minh | *(giọng khẽ)* Cảm ơn mẹ. Con sẽ không phụ lòng mẹ đâu. |
| 7 | Mẹ | Mẹ thương con. Ráng lên nha. |

---

## 🔴 CHỦ NỢ — Brand Voice: *Lịch sự bề ngoài · Leo thang áp lực · Lạnh lùng*

> Tên hiển thị: **"Số lạ"** → **"Anh Tài"**
> Minh trả lời ngắn, cố giữ bình tĩnh nhưng căng dần qua từng cuộc

---

### `CALL_Creditor_Warn1` — Ngày 10 · Nhắc nhẹ

**Trigger:** `currentDay == 10`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Số lạ | Alo, cho hỏi... đây có phải số của chủ tiệm trà không? |
| 2 | Minh | Dạ, tôi nghe. Ai vậy? |
| 3 | Số lạ | À, tôi là Tài, bên phía anh Hùng. Anh nhờ tôi nhắc nhỏ về khoản vay hồi đầu năm. |
| 4 | Minh | Ừ... tôi biết rồi. Tôi đang cố gắng. |
| 5 | Số lạ | Không có gì gấp đâu, chỉ nhắc thôi. Cứ làm ăn tốt là được. Chào. |
| 6 | Minh | *(thở nhẹ)* Chào. |

---

### `CALL_Creditor_Warning` — Ngày 20 · Cảnh báo (capital < 1500)

**Trigger:** `currentDay == 20 && capital < 1500`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Anh Tài | Alo, Tài đây. Tình hình tiệm sao rồi? |
| 2 | Minh | Đang cố... khách chưa đông lắm. |
| 3 | Anh Tài | Anh Hùng nhờ tôi hỏi thăm. Thấy lâu quá chưa thấy động tĩnh gì nên hơi lo. |
| 4 | Minh | Cuối tháng tôi sẽ trả một phần. Anh chờ tôi ít bữa. |
| 5 | Anh Tài | Hạn là cuối tháng này. Chưa đủ thì cũng ráng cho anh một phần, để anh xem lại được. |
| 6 | Minh | Được. Tôi sẽ lo. |
| 7 | Anh Tài | Đừng để anh Hùng phải tự gọi nha. Ổng gọi là khác rồi đó. |
| 8 | Minh | *(im lặng)* Tôi nghe rồi. |

---

### `CALL_Creditor_Threat` — Ngày 30 · Đe dọa (capital < 1000)

**Trigger:** `currentDay == 30 && capital < 1000`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Anh Tài | Tôi gọi lần này là lần cuối đó nghe. |
| 2 | Minh | Tôi biết. Tôi... đang khó khăn thật sự. |
| 3 | Anh Tài | Anh Hùng không muốn làm lớn chuyện, nhưng nếu cuối tuần này không thấy gì thì tôi không hứa được gì nữa. |
| 4 | Minh | Cho tôi thêm một tuần. Tôi xin. |
| 5 | Anh Tài | Tiệm còn đó, tài sản còn đó... Anh biết cách thu hồi. |
| 6 | Minh | *(nghẹn lại)* Đừng... tôi sẽ trả. |
| 7 | Anh Tài | Suy nghĩ kỹ đi. |

---

### `CALL_Creditor_Aggressive` — Ngày 45 · Hung hăng (capital < 500)

**Trigger:** `currentDay == 45 && capital < 500`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Anh Tài | Thôi khỏi nói nhiều. Anh Hùng cho người xuống xem địa chỉ rồi. |
| 2 | Minh | Khoan đã— tôi... |
| 3 | Anh Tài | Trả đủ trong 48 tiếng, không thì tụi tôi tự xử. Đơn giản vậy thôi. |
| 4 | Minh | *(giọng run)* Tôi hiểu rồi. |

---

### `CALL_Creditor_Sweet` — Ngày 30 · Mời vay thêm (capital >= 3000)

**Trigger:** `currentDay == 30 && capital >= 3000`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Anh Tài | Tài đây. Anh Hùng thấy tiệm em làm ăn được, nhờ tôi gọi hỏi thăm. |
| 2 | Minh | Ừ, cũng ổn. Có chuyện gì không anh? |
| 3 | Anh Tài | Ổng có nguồn vốn muốn đầu tư thêm nếu em cần mở rộng. Lãi suất ưu đãi hơn lần trước. |
| 4 | Minh | *(ngập ngừng)* Để tôi nghĩ thêm. Chưa chắc cần. |
| 5 | Anh Tài | Em nghĩ thử đi, cơ hội không phải lúc nào cũng có. Tôi chờ tin. |
| 6 | Minh | Ừ... tôi sẽ liên lạc lại. |

---

## 🟢 BẠN BÈ — Brand Voice: *Hào hứng · Hài hước · Thật lòng*

> Tên hiển thị: **"Hùng"** *(đổi từ Minh để tránh trùng tên chủ tiệm)*
> Speaker: `Hùng` | Minh thoải mái hơn hẳn khi nói chuyện với bạn

---

### `CALL_Friend_Opening` — Ngày 3 · Chúc khai trương

**Trigger:** `currentDay == 3`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Hùng | Ê Minh! Tao nghe mày mở tiệm trà rồi, sao không kêu tao qua phụ khai trương? |
| 2 | Minh | Ủa mày biết rồi á? Tao tưởng chưa kêu ai. |
| 3 | Hùng | Thôi được, tao bỏ qua lần này. Nhưng mà nói thật, tao tự hào mày lắm. Dám làm là ngon rồi. |
| 4 | Minh | Cảm ơn mày. Tao cũng chưa chắc lắm nhưng mà... thử thôi. |
| 5 | Hùng | Cuối tuần tao kéo hội qua ủng hộ nha. Mày có đồ ăn kèm không? Tụi tao đứa nào cũng đói. |
| 6 | Minh | Có bánh đậu xanh thôi. Mà mày kêu hội qua thì tao biết ơn lắm đó. |

---

### `CALL_Friend_Random_A` — Random pool · Hỏi thăm vui vẻ

**Trigger:** ~10% ngẫu nhiên, ngày 5–50

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Hùng | Alo alo, sao rồi ông chủ? Hôm nay bán được nhiều không? |
| 2 | Minh | Tạm được. Hôm nay có mấy khách quen ghé, vui lắm. |
| 3 | Hùng | Tao vừa đi ngang, thấy mày không có mở cửa à. Hay mày đóng sớm vậy? |
| 4 | Minh | Ừ tao đóng sớm, mệt quá. Dọn hàng từ sáng rồi. |
| 5 | Hùng | Nhớ nghỉ ngơi nha, đừng có vắt kiệt sức. |
| 6 | Minh | Biết rồi mày ơi. Mày yên tâm đi. |

---

### `CALL_Friend_Random_B` — Random pool · Tin đồn thị trường

**Trigger:** ~10% ngẫu nhiên, ngày 20–70

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Hùng | Ê tao nghe nói khu đó sắp có mấy quán cà phê chuỗi vô nữa. |
| 2 | Minh | Ừ tao cũng nghe rồi. Cũng lo chút. |
| 3 | Hùng | Mày lo không? Tao thấy bọn đó hay dùng chiêu khuyến mãi lắm. |
| 4 | Minh | Lo thì lo, nhưng khách của tao khác mà. Người ta tìm kiểu trà truyền thống, không phải cà phê chuỗi. |
| 5 | Hùng | Đúng rồi đó! Trà truyền thống khác, mày có cái hồn riêng. Tự tin lên đi! |
| 6 | Minh | Cảm ơn mày. Tao cần nghe câu đó. |

---

### `CALL_Friend_Viral100` — viralScore >= 100 (lần đầu)

**Trigger:** `viralScore >= 100` — trigger 1 lần duy nhất

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Hùng | BRO! Tao vừa thấy tiệm mày trên TikTok! Video đó viral mấy nghìn view rồi! |
| 2 | Minh | Ủa thật không?! Video nào vậy? |
| 3 | Hùng | Người ta quay cảnh mày pha trà, nhìn đẹp lắm. Comment toàn khen không. |
| 4 | Minh | Trời... tao không biết ai quay. Mà vui thật. |
| 5 | Hùng | Tao share cho cả hội rồi. Mày chuẩn bị đón khách lạ nha, sắp đông đó! |
| 6 | Minh | Ừ... tao sẽ chuẩn bị thêm. Cảm ơn mày báo! |

---

### `CALL_Friend_TrueEnding` — Ngày 89 · viralScore >= 1000

**Trigger:** `currentDay == 89 && viralScore >= 1000`

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Hùng | Ê Minh, tao vừa nói chuyện với một người quen bên cơ quan văn hóa quận. |
| 2 | Minh | Ủa? Chuyện gì vậy mày? |
| 3 | Hùng | Họ đang tìm mấy cơ sở buôn bán truyền thống để đưa vào danh sách bảo tồn. Tao kể chuyện mày cho họ rồi. |
| 4 | Minh | Mày làm vậy thiệt á... tao không biết phải nói gì. |
| 5 | Hùng | Ổng nói muốn ghé thăm tiệm. Mày dọn dẹp cho đẹp vào nha, đừng có bẩy nhá. |
| 6 | Minh | Ừ ừ, tao dọn liền. Mà... cảm ơn mày nhiều lắm Hùng ơi. |
| 7 | Hùng | Tao nghĩ đây là cơ hội lớn đó mày. Mày xứng đáng lắm rồi. |
| 8 | Minh | *(xúc động)* Ừ... tao sẽ không bỏ lỡ đâu. |

---

## � CÁC KẾT CỤC (ENDINGS) — Ngày 90

> Kịch bản xảy ra vào đúng **Ngày 90** khi game kiểm tra hàm `CheckEnding()`.

### `ENDING_Bad_OfficeWorker` — Phá sản, về quê làm văn phòng (capital < 5000)

**Nhân vật:** Mẹ
**Ngữ cảnh:** Gọi lúc Minh đang gói đồ dọn dẹp mặt bằng, tiệm chính thức dẹp tiệm.

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Mẹ | Con dọn đồ tới đâu rồi? Có cần mẹ kêu mấy cậu lên phụ một tay không? |
| 2 | Minh | Dạ... sắp xong rồi mẹ. Đồ đạc thanh lý cũng gần hết rồi. |
| 3 | Mẹ | Thôi đừng buồn nữa con. Làm ăn thì có lúc này lúc kia, đâu phải ai mở tiệm cũng thắng ngay đâu. |
| 4 | Minh | Con xin lỗi... con đã làm mất số vốn mẹ cho, mà cũng không giữ được nghề của bà. |
| 5 | Mẹ | Chuyện qua rồi. Mẹ lo cho sức khỏe của con hơn. Về nhà nghỉ ngơi vài hôm, rồi xin vào công ty cũ làm lại cũng được, đúng không? |
| 6 | Minh | ...Dạ. Ngày mai con bắt xe về. |
| 7 | Mẹ | Mẹ nấu sẵn canh dưa chua thịt bò con thích. Về lẹ nha! |

> **Màn hình hiện lên dòng chữ:** *Tiệm Trà chìm vào quên lãng. Minh trở lại với màn hình máy tính và những deadline vô vị nơi công sở...*

---

### `ENDING_Normal_Franchise` — Chuỗi nhượng quyền vô hồn (capital >= 5000, neighborRelation < 80)

**Nhân vật:** Chị Quỳnh (Đại diện công ty nhượng quyền F&B)
**Ngữ cảnh:** Gọi lúc Minh đang ký hợp đồng bán đứt cổ phần tiệm cho tập đoàn.

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Chị Quỳnh | Chào Minh, chị gọi từ phòng pháp chế của tập đoàn. Hợp đồng chuyển nhượng thương hiệu đã gửi qua email em rồi nhé. |
| 2 | Minh | Dạ chị. Em đang coi lại mấy điều khoản cuối. |
| 3 | Chị Quỳnh | Bên chị sẽ chuẩn hóa lại menu. Bỏ bớt mấy dòng trà lá thủ công đi nhé, thay bằng syrup công nghiệp pha sẵn cho nhanh thu hồi vốn. |
| 4 | Minh | Sửa... sửa luôn vị trà cốt lõi của bà ngoại em sao chị? |
| 5 | Chị Quỳnh | Em ơi, kinh doanh chuỗi là phải đồng nhất! Khách hàng gen Z bây giờ chỉ thích ngọt, béo chứ ai uống trà đắng. Em nhận khoản tiền cục tỉ đồng đó thì nhường quyền thiết kế menu cho tụi chị. |
| 6 | Minh | (Thở dài)... Dạ vâng. Em sẽ ký. |
| 7 | Chị Quỳnh | Chốt vậy nhé. Mai chị cho người xuống tháo cái bảng hiệu cũ bằng gỗ xuống, thay bằng đèn LED neon cho hút khách. |

> **Màn hình hiện lên dòng chữ:** *Tiệm Trà phủ sóng khắp cả nước với 50 chi nhánh. Minh trở thành một triệu phú, nhưng mỗi lần uống ly trà công nghiệp ấy, vị đắng của sự mất mát lại hiện lên...*

---

### `ENDING_True_CulturalHeritage` — Di Sản Văn Hóa (neighborRelation >= 80, viralScore >= 1000)

**Nhân vật:** Chú Tám (Tổ trưởng dân phố / Khách hàng thân thuộc)
**Ngữ cảnh:** Ngày trao bằng khen, khu phố ngập tràn hoa và tiếng vỗ tay.

| # | Người nói | Nội dung |
|---|-----------|---------|
| 1 | Chú Tám | Alo thằng Minh! Mày đang ở đâu, sắp tới giờ cắt băng khánh thành cái biển "Điểm Đến Văn Hóa" rổi kìa! |
| 2 | Minh | Dạ dạ chú Tám, con đang thay cái áo sơ mi mới, rối quá! Vụ này lớn thiệt hả chú? |
| 3 | Chú Tám | Lớn chứ sao! Cả cái xóm này tự hào về tiệm mày lắm. Hồi đầu tao thấy mày lóc chóc tưởng dẹp tiệm sớm, ai dè trụ dai mà còn mang tiếng thơm cho khu này. |
| 4 | Minh | (Cười ngượng) Là nhờ mấy cô chú hàng xóm thương, mua ủng hộ con suốt đó chú ơi. |
| 5 | Chú Tám | Cái tiệm mày không chỉ bán nước, nó giữ cái tình người mày hiểu không? Hồi xưa tao qua uống trà với ngoại mày hoài, giờ uống trà mày pha, vị y chang! |
| 6 | Minh | Cảm ơn chú... con vui đến rớt nước mắt. |
| 7 | Chú Tám | Nhanh ra ngoài đi mậy! Đoàn phim của đài truyền hình thành phố dựng xong góc quay rồi, bà con chờ đông lắm! |

> **Màn hình hiện lên dòng chữ:** *Tiệm Trà không trở thành chuỗi khổng lồ, nhưng lại là tài sản vô giá của khu phố. Nó là điểm giao thoa giữa hương vị truyền thống và nhịp sống trẻ gen Z...*

---

## �📋 Tổng hợp asset cần tạo trong Unity

> ⚠️ Nhân vật bạn bè **đổi tên thành "Hùng"** trong tất cả asset

| Asset Name | CallType | Trigger |
|-----------|----------|---------|
| `CALL_Mom_Day1` | Mom | Ngày 1 |
| `CALL_Mom_Day7` | Mom | Ngày 7 |
| `CALL_Mom_Day30_LowCapital` | Mom | Ngày 30, capital < 2000 |
| `CALL_Mom_Day30_HighCapital` | Mom | Ngày 30, capital >= 2000 |
| `CALL_Mom_LowRelation` | Mom | Ngày 60, neighborRelation < 30 |
| `CALL_Mom_Final` | Mom | Ngày 89 |
| `CALL_Creditor_Warn1` | Creditor | Ngày 10 |
| `CALL_Creditor_Warning` | Creditor | Ngày 20, capital < 1500 |
| `CALL_Creditor_Threat` | Creditor | Ngày 30, capital < 1000 |
| `CALL_Creditor_Aggressive` | Creditor | Ngày 45, capital < 500 |
| `CALL_Creditor_Sweet` | Creditor | Ngày 30, capital >= 3000 |
| `CALL_Friend_Opening` | Friend | Ngày 3 |
| `CALL_Friend_Random_A` | Friend | Random 5–50 |
| `CALL_Friend_Random_B` | Friend | Random 20–70 |
| `CALL_Friend_Viral100` | Friend | viralScore >= 100 (1 lần) |
| `CALL_Friend_TrueEnding` | Friend | Ngày 89, viralScore >= 1000 |
