// Modal açma işlemleri  
$(document).ready(function () {
    // Tek bir fonksiyon ile modal form yükleme
    function loadPartial(buttonSelector, url) {
        $(buttonSelector).click(function () {
            $.ajax({
                url: url,
                type: 'GET',
                success: function (data) {
                    $("#myModal .modal-body").html(data);  // Partial view modal içine yerleşir
                    $("#myModal").modal('show');           // Modal açılır
                },
                error: function () {
                    alert("Form yüklenemedi!");
                }
            });
        });
    }

    // 3 buton için çağırıyoruz
    loadPartial("#btnRegister", '/Account/RegisterPartial');
    loadPartial("#btnLogin", '/Account/LoginPartial');
    loadPartial("#btnAdmin", '/Account/AdminPartial');
});


// Form submit işlemleri 
// Kod tekrarı olmaması için tek bir handler
function handleFormSubmit(formSelector, postUrl) {
    $(document).on('submit', formSelector, function (event) {
        event.preventDefault();

        $.ajax({
            url: postUrl,
            type: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    window.location.href = response.redirectUrl;  // Başarılı ise yönlendir
                } else {
                    alert(response.message);  // Hata varsa mesaj göster
                }
            },
            error: function () {
                alert("İşlem sırasında hata oluştu!");
            }
        });
    });
}

// 3 form için tek satırda bağlama
handleFormSubmit('#registerForm', '/Account/Register');
handleFormSubmit('#loginForm', '/Account/Login');
handleFormSubmit('#adminForm', '/Account/AdminLogin');



$(document).ready(function () {
    // Çıkış butonu için AJAX
    $("#btnLogout").click(function () {
        $.post('/Account/Logout', function () {
            window.location.href = '/Home/Index'; // Çıkış sonrası anasayfaya yönlendir
        });
    });
});

// --- Kitap listesini gösterme işlemi ---
// Bu kod, kitap listesini gösteren butona tıklandığında tabloyu görünür yapar
    document.addEventListener("DOMContentLoaded", () => {
    const showButton = document.getElementById("btnList");
    const bookTable = document.getElementById("bookTable");

    showButton.addEventListener("click", () => {
        bookTable.style.display = "table";
    });
});


// Kitap ödünç alma (butona basınca)
function borrowBook(bookId) {
    $.ajax({
        url: '/Borrow/BorrowBook',
        type: 'POST',
        data: { bookId: bookId },
        success: function (response) {
            if (response.success) {
                alert(response.message);
                location.reload(); // Sayfayı yenile, stok güncellensin
            } else {
                alert(response.message);
            }
        },
        error: function () {
            alert("Bir hata oluştu, lütfen tekrar deneyin.");
        }
    });


}
// Ödünç alınan kitapları yükleme
function loadBorrowedBooks() {
            $.ajax({
                url: '/Borrow/BorrowedBooksPartial',
                type: 'GET',
                success: function (data) {
                    $("#borrowedBooksContainer").html(data);
                },
                error: function () {
                    alert("Ödünç alınan kitaplar yüklenemedi!");
                }
            });
        }
        $("#btnBorrowedList").click(function () {
    loadBorrowedBooks();
});
// Ödünç alınan kitapları iade etme

function returnBook(borrowId) {
    $.ajax({
        url: '/Borrow/ReturnBook',
        type: 'POST',
        data: { borrowId: borrowId },
        success: function(response) {
            alert(response.message);
            if (response.success) {
                location.reload();
            }
        },
        error: function() {
            alert('İşlem sırasında hata oluştu!');
        }
    });
}




// Favorilere kitap ekle
function addToFavorites(bookId) {
    $.ajax({
        url: '/Favorite/AddToFavorites',
        type: 'POST',
        data: { bookId: bookId },
        success: function (response) {
            alert(response.message);
            if (response.success) {
                loadFavoriteBooks();
            }
        },
        error: function () {
            alert("Favori eklenirken hata oluştu!");
        }
    });
}

// Favori kitapları yükle
function loadFavoriteBooks() {
    $.ajax({
        url: '/Favorite/FavoriteBooksPartial',
        type: 'GET',
        success: function (data) {
            $("#favoriteBooksContainer").html(data);
        },
        error: function () {
            alert("Favoriler yüklenemedi!");
        }
    });
}

// "Favorilerim" butonuna tıklama
$("#btnFavoriteList").click(function () {
    loadFavoriteBooks();
      });

      //Admin bölümleri**
// Admin kitap listesini gösterme
      document.addEventListener("DOMContentLoaded", () => {
    const adminButton = document.getElementById("btnAdminList");
    const adminTable = document.getElementById("adminBookTable");

    if (adminButton && adminTable) {
        adminButton.addEventListener("click", () => {
            adminTable.style.display = "table";  // Tabloyu görünür yap
        });
    }
});
 document.addEventListener("DOMContentLoaded", () => {
    const adminButton = document.getElementById("btnUsers");
    const adminTable = document.getElementById("adminUserTable");

    if (adminButton && adminTable) {
        adminButton.addEventListener("click", () => {
            adminTable.style.display = "table";  // Tabloyu görünür yap
        });
    }
});

// Admin kitap ekleme
// Modal açma
document.getElementById("btnAddBook").addEventListener("click", () => {
    const modal = new bootstrap.Modal(document.getElementById("addBookModal"));
    modal.show();
});

// Kitap ekleme formu submit
document.getElementById("addBookForm").addEventListener("submit", function (e) {
    e.preventDefault();

    fetch('/Book/Add', {
        method: 'POST',
        body: new URLSearchParams(new FormData(this)),
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        }
    })
    .then(res => res.json())
    .then(data => {
        if (data.success) {
            location.reload(); // Kitap listesi yenilensin
        } else {
            alert(data.message);
        }
    });
});
// Kullanıcı silme
$(document).on('click', '.delete-user', function () {
    var button = $(this); // Tıklanan buton
    var userId = button.data('userid');

    if (!confirm('Bu kullanıcıyı silmek istediğinize emin misiniz?')) {
        return;
    }

    $.ajax({
        url: '/Account/DeleteUser',
        type: 'POST',
        data: { id: userId },
        success: function (response) {
            alert(response.message);
            if (response.success) {
                // Satırı tablodan kaldır
                button.closest('tr').remove();
            }
        },
        error: function () {
            alert('Silme işlemi sırasında hata oluştu.');
        }
    });
});


    


    