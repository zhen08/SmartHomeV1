//
//  AppDelegate.swift
//  Garage
//
//  Created by Zhen Liu on 17/05/17.
//  Copyright © 2017 Zhen Liu. All rights reserved.
//

import UIKit
import ParticleSDK
import MobileCenter
import MobileCenterAnalytics
import MobileCenterCrashes

@UIApplicationMain
class AppDelegate: UIResponder, UIApplicationDelegate {

    var window: UIWindow?


    func application(_ application: UIApplication, didFinishLaunchingWithOptions launchOptions: [UIApplicationLaunchOptionsKey: Any]?) -> Bool {
        // Override point for customization after application launch.
        MSMobileCenter.start("e2b3120c-877c-43ae-9154-93a5756fa562", withServices:[
            MSAnalytics.self,
            MSCrashes.self
            ])
        ParticleCloud.sharedInstance().oAuthClientId="garagedoorapp-48"
        ParticleCloud.sharedInstance().oAuthClientSecret="55ae7be2779a7514e0d7a8ae74d136faa999ee06"
        return true
    }

    func applicationWillResignActive(_ application: UIApplication) {
        // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
        // Use this method to pause ongoing tasks, disable timers, and invalidate graphics rendering callbacks. Games should use this method to pause the game.
    }

    func applicationDidEnterBackground(_ application: UIApplication) {
        // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later.
        // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
    }

    func applicationWillEnterForeground(_ application: UIApplication) {
        // Called as part of the transition from the background to the active state; here you can undo many of the changes made on entering the background.
    }

    func applicationDidBecomeActive(_ application: UIApplication) {
        // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
    }

    func applicationWillTerminate(_ application: UIApplication) {
        // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
    }

}

