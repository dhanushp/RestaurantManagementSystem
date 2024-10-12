function renderPayPalButton(totalAmount) {
    // Clear any previously rendered button if present
    document.getElementById('paypal-button-container').innerHTML = "";

    // Render PayPal button
    paypal.Buttons({
        createOrder: function (data, actions) {
            return actions.order.create({
                purchase_units: [{
                    amount: {
                        value: totalAmount // Dynamic value passed from Blazor
                    }
                }]
            });
        },
        onApprove: function (data, actions) {
            return actions.order.capture().then(function (details) {
                console.log('Transaction completed by ' + details.payer.name.given_name);
                alert('Payment successful!');
                // Optionally: Call your Blazor backend for further actions after successful payment
            });
        },
        onError: function (err) {
            console.error('PayPal Checkout Error', err);
        }
    }).render('#paypal-button-container');
}
