function renderPayPalButton(totalAmount) {
    // Clear any previously rendered button if present
    document.getElementById('paypal-button-container').innerHTML = "";

    const amountString = totalAmount.toFixed(2).toString(); // Use the amount passed 

    // Render PayPal button
    paypal.Buttons({
        createOrder: async function (data, actions) {
            // Make a request to your backend to create the order
            const response = await fetch('https://localhost:5003/api/checkout/create-order', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    amount: amountString, // Pass the amount dynamically here
                    currency: 'USD' // Adjust currency if needed
                })
            });

            const orderData = await response.json();

            if (!response.ok) {
                console.error('Error creating PayPal order:', orderData);
                throw new Error('Could not create order');
            }

            // Return the order ID from your server
            return orderData.data.id; // Adjust if your response structure differs
        },
        onApprove: async function (data, actions) {
            // Call the capture order endpoint
            const captureResponse = await fetch(`https://localhost:5003/api/checkout/capture-order/${data.orderID}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            const captureData = await captureResponse.json();

            if (captureResponse.ok) {
                alert('Payment successful! Transaction ID: ' + captureData.data.id);
            } else {
                alert('Payment failed: ' + captureData.message);
            }
        },
        onError: function (err) {
            console.error('PayPal Checkout Error', err);
        }
    }).render('#paypal-button-container');
}
