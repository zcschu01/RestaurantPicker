# -*- coding: utf-8 -*-
import json, urllib, requests, os

where = urllib.parse.quote_plus("""
{
    "city": "Louisville",
    "state": "Kentucky"
}
""")
url = 'https://parseapi.back4app.com/classes/Usrestaurants_Restaurant?limit=1000&order=name&where=%s' % where
headers = {
    'X-Parse-Application-Id': os.environ['RESTAURANT_APP_ID'], # This is your app's application id
    'X-Parse-REST-API-Key': os.environ['RESTAURANT_API_KEY'] # This is your app's REST API key
}
data = json.loads(requests.get(url, headers=headers).content.decode('utf-8')) # Here you have the data that you need

print("Got JSON\n")

with open("restaurants.txt", "w") as outfile:
    json.dump(data, outfile)
    outfile.close()

print("New file written!")
