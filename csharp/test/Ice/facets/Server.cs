// **********************************************************************
//
// Copyright (c) 2003-2016 ZeroC, Inc. All rights reserved.
//
// This copy of Ice is licensed to you under the terms described in the
// ICE_LICENSE file included in this distribution.
//
// **********************************************************************

using System;
using System.Reflection;

[assembly: CLSCompliant(true)]

[assembly: AssemblyTitle("IceTest")]
[assembly: AssemblyDescription("Ice test")]
[assembly: AssemblyCompany("ZeroC, Inc.")]

public class Server
{
    private static int run(string[] args, Ice.Communicator communicator)
    {
        communicator.getProperties().setProperty("TestAdapter.Endpoints", "default -p 12010");
        Ice.ObjectAdapter adapter = communicator.createObjectAdapter("TestAdapter");
        Ice.Object d = new DI();
        adapter.add(d, Ice.Util.stringToIdentity("d"));
        adapter.addFacet(d, Ice.Util.stringToIdentity("d"), "facetABCD");
        Ice.Object f = new FI();
        adapter.addFacet(f, Ice.Util.stringToIdentity("d"), "facetEF");
        Ice.Object h = new HI(communicator);
        adapter.addFacet(h, Ice.Util.stringToIdentity("d"), "facetGH");

        adapter.activate();
        communicator.waitForShutdown();

        return 0;
    }

    public static int Main(string[] args)
    {
        int status = 0;
        Ice.Communicator communicator = null;

        try
        {
            communicator = Ice.Util.initialize(ref args);
            status = run(args, communicator);
        }
        catch(System.Exception ex)
        {
            System.Console.WriteLine(ex);
            status = 1;
        }

        if(communicator != null)
        {
            try
            {
                communicator.destroy();
            }
            catch(Ice.LocalException ex)
            {
                System.Console.WriteLine(ex);
                status = 1;
            }
        }

        return status;
    }
}
