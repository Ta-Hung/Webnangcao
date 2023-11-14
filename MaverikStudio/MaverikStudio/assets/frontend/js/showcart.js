function formatCurrency(number) {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND', useGrouping: true }).format(number);
}

function getCartFromCookie() {
    var gioHangCookieValue = document.cookie.replace(/(?:(?:^|.*;\s*)gioHangCookie\s*\=\s*([^;]*).*$)|^.*$/, "$1");

    // Kiểm tra nếu chuỗi có nghĩa trước khi parse
    if (gioHangCookieValue) {
        try {
            // Giải mã giá trị cookie từ JSON
            var gioHang = JSON.parse(decodeURIComponent(gioHangCookieValue));

            // Nếu giỏ hàng không tồn tại, trả về một mảng rỗng
            return gioHang || [];
        } catch (error) {
            console.error("Lỗi khi parse JSON từ cookie:", error);
        }
    }

    // Nếu có lỗi hoặc cookie không tồn tại, trả về một mảng rỗng
    return [];
}

const cart = $('.header__middle-container-right-form-cart-view');

cart.click(function(e) {
    if(e.target.classList.contains('delete-cart-item')) {
        console.log(e.target.getAttribute('data-id'));
        removeFromCart(e.target.getAttribute('data-id'),e.target.getAttribute('data-size'));
        showCart();
    }
})

function showCart() {
    let arrItemCart = getCartFromCookie();
    let totalMoney = 0;
    if(arrItemCart.length > 0) {
        cart.html(arrItemCart.map(function(item) {
            totalMoney += item.quantity*item.price;
            return `
                <div class="cart-item">
                    <div class="cart-item-thumbs">
                        <a href="/product/${item.id}" style="background-image: url('${item.image}');"></a>
                    </div>
                    <div class="cart-item-info">
                        <div style="max-width: 90%;">
                            <a href="#">${item.name}</a>
                        </div>
                        <p>${item.size}</p>
                        <p>${item.quantity}</p>
                        <p>${formatCurrency(item.quantity * item.price)}</p>
                        <div data-id="${item.id}" data-size="${item.size_id}" class="delete-cart-item"></div>
                    </div>
                </div>
            `;
        }).join(' '));
    } else {
        cart.html(`
            <div class="null-cart">
                <i class="fa-light fa-cart-xmark"></i>
                <p>Hiện chưa có sản phẩm</p>
            </div>
        `);
    }

    $('#cartQuantityItem').text(arrItemCart.length);
    $('#cartTotalMoney').text(formatCurrency(totalMoney));
}

showCart();