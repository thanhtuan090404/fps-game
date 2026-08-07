# CHECKLIST TRƯỚC KHI COMMIT

Mở file này mỗi lần trước khi chạy `git add`. Đọc hết, không lướt.

## 1. Rác cần dọn
- [ ] Xoá hết `Debug.Log` dùng để săn bug (`Health Awake: 0`, `Attack trigger fired!`, ...)
- [ ] Không còn `Debug.Log` nào chạy trong `Update` / mỗi frame
- [ ] Xoá `using` thừa (VS Code tô mờ chúng — Ctrl+. để bỏ)
- [ ] Không còn code chết, không còn component bị tắt tay để "vá tạm"

## 2. Đặt tên
- [ ] Private field: `_camelCase`  (`_musicSource`, không phải `musicSource`)
- [ ] Public / property: `PascalCase`  (`CurrentHealthPercent`)
- [ ] Hàm: `PascalCase`, động từ  (`TakeDamage`, không phải `takedamage`)
- [ ] Tên đúng chính tả: TakeDamage / isStopped / Canvas / Awake

## 3. Đóng gói dữ liệu
- [ ] KHÔNG có `public` field — dùng property `{ get; private set; }`
- [ ] `[SerializeField]` chỉ cho biến CẤU HÌNH (maxHealth, damage, reloadTime)
- [ ] `[SerializeField]` KHÔNG dùng cho biến RUNTIME (currentAmmo, currentEnemy)
- [ ] Không có magic number — số nào cũng phải có tên biến

## 4. Event  ⚠️ (nhóm lỗi nguy hiểm nhất)
- [ ] Mỗi `+=` có đúng MỘT `-=` đối xứng
- [ ] Đăng ký ở `OnEnable`, huỷ ở `OnDisable` (không phải Start/OnDestroy)
- [ ] `static event` bắt buộc phải unsubscribe — không ai dọn hộ
- [ ] Có `?.Invoke()`, không gọi `Invoke()` trần

## 5. Vòng đời & hiệu năng
- [ ] `Awake` = tự chuẩn bị dữ liệu mình. `Start` = nối với script khác
- [ ] KHÔNG `GetComponent` trong `Update` — cache ở `Awake`
- [ ] Physics nằm trong `FixedUpdate`
- [ ] Coroutine có guard flag, và `OnDisable` reset cờ đó

## 6. Singleton & Manager  (Ngày 29)
- [ ] Singleton persistent nằm MỘT MÌNH trên GameObject gốc
- [ ] Có đủ: kiểm tra trùng → `Destroy(gameObject)` → `return;`
- [ ] Có `OnDestroy` gán `Instance = null`
- [ ] Manager giữ state của một ván chơi thì KHÔNG persistent
- [ ] Transform của manager ở (0, 0, 0)

## 7. Bẫy hay dính
- [ ] Không so sánh `float == 0` — dùng `Mathf.Approximately`
- [ ] Animator: đấm = Trigger, chết = Bool. Không đổi chỗ
- [ ] Comment đọc lại xem còn ĐÚNG không (AddSkill vs AddKill, "5 giây" khi biến = 1)
- [ ] `Time.timeScale` reset về 1 trước khi `LoadScene`

## 8. Git
- [ ] Đã chạy `git status` và `git diff`, đọc từng dòng thay đổi
- [ ] Đúng branch (NHÌN NGOẶC VÀNG)
- [ ] Commit message theo Conventional Commits: `feat:` `fix:` `refactor:` `chore:` `docs:`
- [ ] Không commit: Library/ Temp/ Build/ *.exe
- [ ] Đọc DÒNG CUỐI output mỗi lệnh — thấy `Aborting` / `error` / `up-to-date` bất thường = CHƯA xong
## 4. Event
- [ ] Manager mới cần dữ liệu từ event → nó TỰ đăng ký, không nhờ manager khác gọi hộ

## 3. Đóng gói dữ liệu
- [ ] Thấy mình viết `if (XxxManager.Instance != null)` → dừng lại, hỏi: thiết kế có sai không?.