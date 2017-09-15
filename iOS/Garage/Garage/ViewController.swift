//
//  ViewController.swift
//  Garage
//
//  Created by Zhen Liu on 17/05/17.
//  Copyright © 2017 Zhen Liu. All rights reserved.
//

import UIKit
import SystemConfiguration.CaptiveNetwork
import ParticleSDK

class ViewController: UIViewController {

    @IBOutlet weak var activityIndicator: UIActivityIndicatorView!
    @IBOutlet weak var messageLabel: UILabel!
    @IBOutlet weak var button: UIButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        self.button.isEnabled = false
        messageLabel.text = ""
        activityIndicator.hidesWhenStopped = true
        NotificationCenter.default.addObserver(self,
                                               selector: #selector(appWillEnterForeground),
                                               name: NSNotification.Name.UIApplicationWillEnterForeground,
                                               object: nil)
        login()
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }

    @IBAction func buttonPressed(_ sender: Any) {
        if (!atHome()){
            let alert = UIAlertController(title: "Alert", message: "Are you really at home?", preferredStyle: UIAlertControllerStyle.alert)
            alert.addAction(UIAlertAction(title: "No", style: UIAlertActionStyle.default, handler: nil))
            alert.addAction(UIAlertAction(title: "Yes", style: UIAlertActionStyle.default, handler: {action in
                self.remoteButtonPress()
            }))
            self.present(alert, animated: true, completion: nil)
        } else {
            remoteButtonPress();
        }
    }
    
    func appWillEnterForeground() {
        login();
    }

    func login(){
        if (ParticleCloud.sharedInstance().isAuthenticated) {
            self.messageLabel.text = "Ready"
            self.button.isEnabled = true
            return
        }
        self.messageLabel.text = "Signing in"
        ParticleCloud.sharedInstance().login(withUser: "zhen08@gmail.com", password: "iloveu44") {
            (error:Error?) -> Void in
            if let _ = error {
                DispatchQueue.main.asyncAfter(deadline: .now() + 0.1) {
                    self.messageLabel.text = "Wrong credentials or no internet connectivity"
                }
            }
            else {
                DispatchQueue.main.asyncAfter(deadline: .now() + 0.1) {
                    self.messageLabel.text = "Ready"
                    self.button.isEnabled = true
                }
            }
        }
    }

    func remoteButtonPress()
    {
        self.button.isEnabled = false
        self.activityIndicator.startAnimating()
        ParticleCloud.sharedInstance().getDevice("200044000851353531343431") { (device:ParticleDevice?, err:Error?) in
            if (device != nil){
                device!.callFunction("remotebutton", withArguments: [""], completion: { (resultCode : NSNumber?, error : Error?) -> Void in
                    DispatchQueue.main.asyncAfter(deadline: .now() + 0.1) {
                        self.activityIndicator.stopAnimating()
                        self.button.isEnabled = true
                        if (error == nil) {
                            self.messageLabel.text = "Done!"
                        } else {
                            self.messageLabel.text = error!.localizedDescription
                        }
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
                    return (interfaceData["SSID"] as! String == "ZL1JI-AC")||(interfaceData["SSID"] as! String == "ZL1JI-AC 5GHz")
                }
            }
        }
        print("Not at home!")
        return false
    }

}

