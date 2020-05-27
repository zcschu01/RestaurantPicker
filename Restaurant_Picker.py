# -*- coding: utf-8 -*-
import json
import urllib
import requests

where = urllib.parse.quote_plus("""
{
    "city": "Louisville",
    "state": "Kentucky"
}
""")
url = 'https://parseapi.back4app.com/classes/Usrestaurants_Restaurant?limit=1000&order=name&where=%s' % where
headers = {
    'X-Parse-Application-Id': 'lS6y8Hxj2ebqdINzyx0BC61m4zZfN65IYntUQoOD', # This is your app's application id
    'X-Parse-REST-API-Key': 'ri2wLRYJjb18tVgBjuW58evJi8D7DsShjBjnRUpo' # This is your app's REST API key
}
data = json.loads(requests.get(url, headers=headers).content.decode('utf-8')) # Here you have the data that you need

print("Got JSON\n")

with open("restaurants.txt", "w") as outfile:
    json.dump(data, outfile)
    outfile.close()

print("New file written!")
