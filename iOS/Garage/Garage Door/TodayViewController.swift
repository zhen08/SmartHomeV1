//
//  TodayViewController.swift
//  Garage Door
//
//  Created by Zhen Liu on 17/05/17.
//  Copyright © 2017 Zhen Liu. All rights reserved.
//

import UIKit
import NotificationCenter
import SystemConfiguration.CaptiveNetwork
import ParticleSDK

class TodayViewController: UIViewController, NCWidgetProviding {
        
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view from its nib.
        ParticleCloud.sharedInstance().oAuthClientId="" //Client ID
        ParticleCloud.sharedInstance().oAuthClientSecret="" //Client Secret
        login()
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    func widgetPerformUpdate(completionHandler: (@escaping (NCUpdateResult) -> Void)) {
        login()
        
        completionHandler(NCUpdateResult.newData)
    }
    
    @IBAction func buttonPressed(_ sender: Any) {
        if (!atHome()){
            return
        }
        ParticleCloud.sharedInstance().getDevice("deviceid") { (device:ParticleDevice?, err:Error?) in
            if (device != nil){
                device!.callFunction("remotebutton", withArguments: [""], completion: { (resultCode : NSNumber?, error : Error?) -> Void in
                    if (error == nil) {
                        print("Button pressed successfully")
                    } else {
                        print(error!.localizedDescription)
                    }
                })
            }
        }
    }

    func atHome() -> Bool {
        if let interface = CNCopySupportedInterfaces() {
            for i in 0..<CFArrayGetCount(interface) {
                let interfaceName: UnsafeRawPointer = CFArrayGetValueAtIndex(interface, i)
                let rec = unsafeBitCast(interfaceName, to: AnyObject.self)
                if let unsafeInterfaceData = CNCopyCurrentNetworkInfo("\(rec)" as CFString), let interfaceData = unsafeInterfaceData as? [String : AnyObject] {
                    return interfaceData["SSID"] as! String == "ZL1JI-AC"
                }
            }
        }
        print("Not at home!")
        return false
    }

    func login(){
        if (ParticleCloud.sharedInstance().isAuthenticated) {
            return
        }
        ParticleCloud.sharedInstance().login(withUser: "zhen08@gmail.com", password: "iloveu44") {
            (error:Error?) -> Void in
            if let _ = error {
                print("Wrong credentials or no internet connectivity, please try again")
            }
            else {
                print("Logged in")
            }
        }
    }
}
