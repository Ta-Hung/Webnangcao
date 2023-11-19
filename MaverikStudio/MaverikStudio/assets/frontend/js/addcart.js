$(function () {
    $('.products__detail-content-right-size-watch-element').click(function () {
        $('.products__detail-content-right-size-watch-element').removeClass('selected');
        $(this).addClass('selected');
        let idProduct = $(this).data('id');
        let idSize = $(this).data('size_id');
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
                    console.log("a", idProduct, idSize)
                    $("#hethang").addClass("abc_hide");
                    $("#hethang").removeClass("abc_show");
                    $("#themgio").addClass("abc_show");
                    $("#themgio").removeClass("abc_hide");
                    $("#muangay").addClass("abc_show");
                    $("#muangay").removeClass("abc_hide");
                } else {
                    console.log("b")
                    $("#hethang").removeClass("abc_hide");
                    $("#hethang").addClass("abc_show");
                    $("#themgio").removeClass("abc_show");
                    $("#themgio").addClass("abc_hide");
                    $("#muangay").removeClass("abc_show");
                    $("#muangay").addClass("abc_hide");
                }
            },
            error: function (error) {
                console.error("Error:", error);
            }
        });
    })

    $('.products__detail-content-right-size-watch-element:first-of-type').click();

    let inputQuantity = $('.quantity-value');

    $('.cong').click(function () {
        if (inputQuantity.val() > 0) {
            inputQuantity.val(1 + parseInt(inputQuantity.val()));
        } else {
            inputQuantity.val(1);
        }
    })

    $('.tru').click(function () {
        if (inputQuantity.val() > 1) {
            inputQuantity.val(parseInt(inputQuantity.val()) - 1);
        } else {
            inputQuantity.val(1);
        }
    })

    inputQuantity.on('change', function () {
        if ($(this).val() > 0) {
            
        } else {
            $(this).val(1);
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

                    if (quantity > 0) {
                        removeFromCart(id, size_id);
                        addToCart(id, size_id, size, name, price, quantity, image);
                        showCart();

                        iconCart.click();
                    }
                } else {
                    alert("So luong ton kho khong du");
                }
            },
            error: function (error) {
                console.error("Error:", error);
            }
        });
    })

    $('#muangay').click(function (e) {
        e.preventDefault();
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

                    if (quantity > 0) {
                        removeFromCart(id, size_id);
                        addToCart(id, size_id, size, name, price, quantity, image);
                        showCart();

                        window.location.href = '/FrontEnd/Pay';
                    }
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
    gioHang = gioHang.filter(function (item) {
        if (item.id == id && item.size_id == size_id) {
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
