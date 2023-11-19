$(function() {
    $('.products__detail-content-right-size-watch-element').click(function() {
        $('.products__detail-content-right-size-watch-element').removeClass('selected');
        $(this).addClass('selected');
    })

    let inputQuantity = $('.quantity-value');

    $('.cong').click(function() {
        inputQuantity.val(1+ parseInt(inputQuantity.val()));
    })

    $('.tru').click(function() {
        if(inputQuantity.val() > 0) {
            inputQuantity.val(parseInt(inputQuantity.val()) -1);
        }
    })

    inputQuantity.on('change', function() {
        if($(this).val() < 0) {
            $(this).val(0);
        }
    })

    $('.btn-addcart.button-dark.them').click(function () {
        $.ajax({
            url: '/FrontEnd/CheckQuantity',
            method: 'GET',
            data: {
                id: $('h1[data-id]').data('id'),
                size_id: $('.products__detail-content-right-size-watch-element.selected').data('id'),
                quantity: $('input.quantity-value').val()
            },
            success: function (data) {
                if (data.check == "true") {
                    let id = $('h1[data-id]').data('id');
                    let name = $('h1[data-id]').data('name');
                    let price = $('h1[data-id]').data('price');
                    let size_id = $('.products__detail-content-right-size-watch-element.selected').data('id');
                    let size = $('.products__detail-content-right-size-watch-element.selected').data('size');
                    let quantity = $('input.quantity-value').val()
                    let image = $('[url-image-pc]').attr('url-image-pc');

                    removeFromCart(id, size_id);
                    addToCart(id, size_id, size, name, price, quantity, image);
                    showCart();

                    iconCart.click();
                } else {
                    alert("So luong ton kho khong du");
                }
            },
            error: function (error) {
                console.error("Error:", error);
            }
        });
    })
})



function addToCart(id, size_id, size, nameProduct, price, quantity, image) {
    // Lấy giỏ hàng từ cookie (nếu có)
    var gioHang = getCartFromCookie();

    

    // Thêm sản phẩm mới vào giỏ hàng
    var newItem = { id: id, size_id: size_id, size: size, name: nameProduct, price: price, quantity: quantity, image: image };
    gioHang.push(newItem);

    // Lưu giỏ hàng mới vào cookie
    setCartToCookie(gioHang);
}

function removeFromCart(id, size_id) {
    // Lấy giỏ hàng từ cookie
    var gioHang = getCartFromCookie();

    // Tìm và xóa sản phẩm khỏi giỏ hàng dựa trên tên sản phẩm
    gioHang = gioHang.filter(function(item) {
        if(item.id == id && item.size_id == size_id) {
            return false;
        }
        return true;
    });

    // Cập nhật thông tin giỏ hàng trong cookie
    setCartToCookie(gioHang);
}

function setCartToCookie(gioHang) {
    // Mã hóa giá trị giỏ hàng thành JSON
    var gioHangJson = encodeURIComponent(JSON.stringify(gioHang));

    // Thiết lập cookie với tên "gioHangCookie" và giá trị là giỏ hàng đã được mã hóa
    document.cookie = "gioHangCookie=" + gioHangJson + "; path=/; SameSite=Lax";
}
