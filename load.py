import requests
import json

"""
Massive disclaimer: I'm not a python dev!!!!

If you know better than me make it better :)
"""


def plugin_start():
    print("Elite-Ingress Initialised...")
    return 'OldBoys Elite Uploader'

def plugin_stop():
    """
    EDMC is closing
    """
    print("EPP-RTS shutting down...")

def journal_entry(cmdr, is_beta, system, station, entry, state):
    #if entry['event'] == 'FSDJump':
        # We arrived at a new system!
# just smash the raw data to the website, we'll take care of it there...

    ignoreEvents = ["Music"] #Update this as needed

    if entry['event'] in ignoreEvents:
        return # nothing to do

    print("EliteIngress: Handling {} event".format(entry["event"]))

    requestBody = {
        'commander': cmdr,
        'data': entry
    }

    #print(requestBody)

    r = requests.put("https://epprts1.wokket.com/", json=requestBody, headers={'Content-Type': 'application/json'})
    print(r.status_code)
