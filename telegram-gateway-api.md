Telegram Gateway API

The Gateway API is an HTTP-based interface created for developers looking to deliver automated messages, such as verification codes, to users who registered their phone number on Telegram.

This page outlines the full API documentation for developers. For more information on the API and the features it offers, see our Verification Platform Overview and Gateway API Tutorial.
Recent changes

February 26, 2025
Updated the possible values for ttl in sendVerificationMessage. The supported range is now 30 to 3600 seconds.
Clarified the behavior of ttl:
If a message is not delivered within the specified ttl, the request fee will be refunded automatically.
If a message is successfully delivered within the ttl, it will not be refunded.
If you were already using ttl before this update, you do not need to change anything to receive refunds.
Updated revokeVerificationMessage to specify that a message will not be removed if it has already been delivered or read.
Added the optional field is_refunded to RequestStatus, which indicates whether the request fee was refunded.
Added new possible statuses delivered and expired to the field status in DeliveryStatus.
Making requests

All queries to the Telegram Gateway API must be served over HTTPS and need to be presented in this form: https://gatewayapi.telegram.org/METHOD_NAME. Like this for example:

https://gatewayapi.telegram.org/sendVerificationMessage
We support GET and POST HTTP methods. We support three ways of passing parameters in Gateway API requests:

URL query string
application/x-www-form-urlencoded
application/json
The response contains a JSON object, which always has a Boolean field ok. If ok equals true, the request was successful, and the result of the query can be found in the result field. In case of an unsuccessful request, ok equals false, and the error is explained in the error field (e.g. ACCESS_TOKEN_INVALID).

All methods in the Gateway API are case-insensitive.
All queries must be made using UTF-8.
Authorization

Before invoking API methods, you must obtain an access token in the Telegram Gateway account settings.

The token must be passed in every request in one of two ways:

in the HTTP header: Authorization: Bearer <token>
as the access_token parameter.
Available methods

We support GET and POST HTTP methods. Use either URL query string or application/json or application/x-www-form-urlencoded for passing parameters in Telegram Gateway API requests.
On successful call, a JSON object containing the result will be returned.
sendVerificationMessage
Use this method to send a verification message. Charges will apply according to the pricing plan for each successful message delivery. Note that this method is always free of charge when used to send codes to your own phone number. On success, returns a RequestStatus object.

See the tutorial for examples >
Parameter	Type	Required	Description
phone_number	String	Yes	The phone number to which you want to send a verification message, in the E.164 format.
request_id	String	Optional	The unique identifier of a previous request from checkSendAbility. If provided, this request will be free of charge.
sender_username	String	Optional	Username of the Telegram channel from which the code will be sent. The specified channel, if any, must be verified and owned by the same account who owns the Gateway API token.
code	String	Optional	The verification code. Use this parameter if you want to set the verification code yourself. Only fully numeric strings between 4 and 8 characters in length are supported. If this parameter is set, code_length is ignored.
code_length	Integer	Optional	The length of the verification code if Telegram needs to generate it for you. Supported values are from 4 to 8. This is only relevant if you are not using the code parameter to set your own code. Use the checkVerificationStatus method with the code parameter to verify the code entered by the user.
callback_url	String	Optional	An HTTPS URL where you want to receive delivery reports related to the sent message, 0-256 bytes.
payload	String	Optional	Custom payload, 0-128 bytes. This will not be displayed to the user, use it for your internal processes.
ttl	Integer	Optional	Time-to-live (in seconds) before the message expires. If the message is not delivered or read within this time, the request fee will be refunded. Supported values are from 30 to 3600.
checkSendAbility
Use this method to optionally check the ability to send a verification message to the specified phone number. If the ability to send is confirmed, a fee will apply according to the pricing plan. After checking, you can send a verification message using the sendVerificationMessage method, providing the request_id from this response.

Within the scope of a request_id, only one fee can be charged. Calling sendVerificationMessage once with the returned request_id will be free of charge, while repeated calls will result in an error. Conversely, calls that don't include a request_id will spawn new requests and incur the respective fees accordingly. Note that this method is always free of charge when used to send codes to your own phone number.

In case the message can be sent, returns a RequestStatus object. Otherwise, an appropriate error will be returned.

See the tutorial for examples >
Parameter	Type	Required	Description
phone_number	String	Yes	The phone number for which you want to check our ability to send a verification message, in the E.164 format.
checkVerificationStatus
Use this method to check the status of a verification message that was sent previously. If the code was generated by Telegram for you, you can also verify the correctness of the code entered by the user using this method. Even if you set the code yourself, it is recommended to call this method after the user has successfully entered the code, passing the correct code in the code parameter, so that we can track the conversion rate of your verifications. On success, returns a RequestStatus object.

See the tutorial for examples >
Parameter	Type	Required	Description
request_id	String	Yes	The unique identifier of the verification request whose status you want to check.
code	String	Optional	The code entered by the user. If provided, the method checks if the code is valid for the relevant request.
revokeVerificationMessage
Use this method to revoke a verification message that was sent previously. Returns True if the revocation request was received. However, this does not guarantee that the message will be deleted. For example, if the message has already been delivered or read, it will not be removed.

Parameter	Type	Required	Description
request_id	String	Yes	The unique identifier of the request whose verification message you want to revoke.
Available types

All types used in Telegram Gateway API responses are represented as JSON objects.

It is safe to use 32-bit signed integers for storing all Integer fields unless otherwise noted.

Optional fields may be not returned when irrelevant.
RequestStatus
This object represents the status of a verification message request.

Field	Type	Description
request_id	String	Unique identifier of the verification request.
phone_number	String	The phone number to which the verification code was sent, in the E.164 format.
request_cost	Float	Total request cost incurred by either checkSendAbility or sendVerificationMessage.
is_refunded	Boolean	Optional. If True, the request fee was refunded.
remaining_balance	Float	Optional. Remaining balance in credits. Returned only in response to a request that incurs a charge.
delivery_status	DeliveryStatus	Optional. The current message delivery status. Returned only if a verification message was sent to the user.
verification_status	VerificationStatus	Optional. The current status of the verification process.
payload	String	Optional. Custom payload if it was provided in the request, 0-256 bytes.
DeliveryStatus
This object represents the delivery status of a message.

Field	Type	Description
status	String	The current status of the message. One of the following:
- sent – the message has been sent to the recipient's device(s),
- delivered – the message has been delivered to the recipient's device(s),
- read – the message has been read by the recipient,
- expired – the message has expired without being delivered or read,
- revoked – the message has been revoked.
updated_at	Integer	The timestamp when the status was last updated.
VerificationStatus
This object represents the verification status of a code.

Field	Type	Description
status	String	The current status of the verification process. One of the following:
- code_valid – the code entered by the user is correct,
- code_invalid – the code entered by the user is incorrect,
- code_max_attempts_exceeded – the maximum number of attempts to enter the code has been exceeded,
- expired – the code has expired and can no longer be used for verification.
updated_at	Integer	The timestamp for this particular status. Represents the time when the status was last updated.
code_entered	String	Optional. The code entered by the user.
Report delivery

The Telegram Gateway API can send delivery reports to a user-specified callback URL. When you include a callback_url parameter in your request, the API will send an HTTP POST request to that URL containing the delivery report for the message. The payload of the POST request will be a JSON object representing the RequestStatus object.

Your URL must respond with HTTP status code 200 to acknowledge receipt of the report. Any other status code will be considered a failure, and the service will retry sending the same report up to 10 times with increasing delays between attempts. If all retries fail, the report will be considered lost.

Checking report integrity
All reports submitted to your callback_url, if you provided one, will also contain the following headers:

X-Request-Timestamp – A Unix timestamp indicating when the server submitted the report.
X-Request-Signature – A server-generated signature needed to authenticate the report on your end.
You can confirm the origin and verify the integrity of the reports you receive by comparing the signature contained in the X-Request-Signature header with the hexadecimal representation of the HMAC-SHA-256 signature of the data-check-string with the SHA256 hash of the API token shown in your Gateway account settings.

The data-check-string is a concatenation of the report timestamp as provided by the X-Request-Timestamp header, a line feed character ('\n', 0x0A) used as separator and the raw post body of the HTTP request.

Example:

data_check_string = X-Request-Timestamp + '\n' + post_body
secret_key = SHA256(api_token)
if (hex(HMAC_SHA256(data_check_string, secret_key)) == X-Request-Signature) {
  // data is from Telegram
}
To prevent the use of outdated data, you should additionally check the X-Request-Timestamp header, which contains a Unix timestamp of when the relevant report was submitted by the server.

Authorization via Telegram Gateway: Quick-start Guide

The Telegram Gateway API allows any service to send authorization codes through Telegram instead of traditional SMS, offering a more affordable, secure, and reliable alternative. This guide will help you quickly integrate the API into your service. For more information, see:

Full Gateway API Reference for Developers
Gateway Overview
Gateway FAQ
Gateway Terms of Service
Index

Before you Start
Sending Auth Codes
Checking the Authorization Status
Receiving Reports
Before you Start

TLDR: open this page and log in with your Telegram phone number, then fund your account here and note down your API token.
Before getting started, you'll need to create an account on our dedicated gateway platform. To do this, navigate to the Gateway platform and click ‘Log in to Start’, then confirm your login via Telegram. If this is your first time using the platform, you'll be prompted to provide some basic information about yourself and your business.

For testing purposes, you’ll be able to send free verification messages to the Telegram account tied to the number you used to log in.
Funding your Account
To send messages to other Telegram users via the Gateway API, you’ll need to fund your account. To do so, simply navigate to this page and click ‘Add Funds on Fragment’, then follow the instructions on Fragment.

On the same page you’ll find a detailed transaction history showing your deposits and expenses.

Obtaining your API Token
Before invoking API methods, you must obtain an access token in your Telegram Gateway account settings. To do so, open this page and click ‘Copy Token’.

The token must be passed in every request in one of two ways:

In the HTTP header: Authorization: Bearer <token>
As the access_token parameter.
In this guide, we’ll assume you decided to use a bearer token.

To increase security, you can limit which IPs or IP ranges are allowed to use your API token from this page.
Querying the API
The API can be queried with GET and POST HTTP methods with your language or framework of choice. Throughout this guide, we’ll use Python as an example and assume you defined the following:

import requests

# Base API url, your Gateway API token and a phone number
BASE_URL = 'https://gatewayapi.telegram.org/'
TOKEN = 'AAEFAAAAQKI_mDsJppSEQRr3kLOz9SatBxq48BgQLSHLRv'
PHONE = '+391234567890'
HEADERS = {
    'Authorization': f'Bearer {TOKEN}',
    'Content-Type': 'application/json'
}

# Function to query the API
def post_request_status(endpoint, json_body):
    url = f"{BASE_URL}{endpoint}"
    response = requests.post(url, headers=HEADERS, json=json_body)
    if response.status_code == 200:
        response_json = response.json()
        if response_json.get('ok'):
            res = response_json.get('result', {})
            return res
        else:
            error_message = response_json.get('error', 'Unknown error')
            print(f"Error: {error_message}")
            return None
    else:
        print(f"Failed to get request status: HTTP {response.status_code}")
        return None
Sending Auth Codes

TLDR: Pass a phone number (E.164 format) to this method to directly send an auth code to the user. If you need a dry run to verify your code can be delivered, use this method.
To send the user a code generated by Telegram, you can use the sendVerificationMessage method:

endpoint = 'sendVerificationMessage'

json_body = {
    'phone_number': PHONE,         # Must be the one tied to request_id
    'code_length': 6,              # Ignored if you specify your own 'code'
    'ttl': 60,                     # 1 minute
    'payload': 'my_payload_here',  # Not shown to users
    'callback_url': 'https://my.webhook.here/auth'
}

result = post_request_status(endpoint, json_body)
If the method returns an error (e.g., because the user cannot receive codes), you will not be charged. On success, it will return a RequestStatus object.
You can optionally specify the sender_username parameter, designating a verified channel you own as the one from which the code will be sent to the user. When sending codes to yourself for testing purposes, the channel does not need to be verified.

Checking that codes can be delivered
Before sending, you can also optionally use the checkSendAbility method to verify that the user you want to target can receive messages on Telegram. This is especially useful when you generated a code yourself but need to check that it can be delivered before sending it out to the user through the Gateway API.

endpoint = 'checkSendAbility'

json_body = {
    'phone_number': PHONE # E.164 format
}

result = post_request_status(endpoint, json_body)

if result:
    request_id = result.get('request_id')
    print(f"Request ID: {request_id}")
You will be automatically charged in advance for the price of one message if the method indicates that the user can be contacted – regardless of whether you eventually send one. If the method returns an error (e.g., because the user cannot receive codes), you will not be charged.
If the check succeeds and you ultimately decide to send a message, use the request_id returned by the checkSendAbility method to avoid paying the message fee again.

Revoking Codes
You can optionally revoke codes even before they expire by passing the relevant request_id to the revokeVerificationMessage method. Messages which were already read will not be deleted.

Note that revoked codes are not refunded. If you need to send codes free of charge for testing purposes, direct them to your own phone number.
Checking the Authorization Status

If you let Telegram generate a code for you (i.e., you did not provide a code param in your request), you can use the checkVerificationStatus method to verify the OTP submitted by your user.

endpoint = 'checkVerificationStatus'

json_body = {
    'request_id': request_id, # Relevant request id
    'code': CODE,             # The code the user entered in your app
}

result = post_request_status(endpoint, json_body)

# Assuming the request was successful
status = result.get('verification_status', {}).get('status')
print(status == 'code_valid') # True if the user entered the correct code
Even if you set the code yourself, you should call this method after the user has entered a code, so that we can display accurate statistics on your Gateway interface.
Receiving Reports

When you include a callback_url parameter in your sendVerificationMessage request, the Gateway API will send a POST request to that URL containing a delivery report for the message. The payload of the POST request will be a JSON object representing the relevant RequestStatus object.
When processing reports, you must verify the integrity of the data you receive as documented here.

Note: The latest API documentation is available here, with in-depth explanations for each method and parameter. This guide is intended to get you started quickly and assumes you’re familiar with programming, correctly handling errors, adequately securing and storing phone numbers, responses and credentials.