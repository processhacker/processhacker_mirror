<?php $pagetitle = "FAQ"; include "header.php"; ?>

<div class="page">
    <div class="yui-d0">
        <div class="yui-t4">
            <nav>
                <div class="logo">
                    <a href="/"><img class="flowed-block" src="img/logo_64x64.png" alt="Project Logo" width="64" height="64"></a>
                </div>
                <div class="flowed-block">
                    <h2>Process Hacker</h2>
                    <ul class="facetmenu">
                        <li><a href="/">Overview</a></li>
                        <li><a href="downloads.php">Downloads</a></li>
                        <li class="active"><a href="faq.php">FAQ</a></li>
                        <li><a href="about.php">About</a></li>
                        <li><a href="http://wj32.org/processhacker/forums/">Forum</a></li>
                    </ul>
                </div>
            </nav>

            <p></p><!-- this p div is a placeholder -->

            <div class="pre-section">
                <div class="yui-b side">
                    <div class="">
                    </div>
                    <div class="portlet">
                        <h2 class="center">Quick Links</h2>
                        <ul class="involvement">
                            <li><a href="http://sourceforge.net/projects/processhacker/">SourceForge project page</a></li>
                            <li><a href="http://wj32.org/processhacker/forums/viewforum.php?f=5">Ask a question</a></li>
                            <li><a href="http://wj32.org/processhacker/forums/viewforum.php?f=24">Report a bug</a></li>
                            <li><a href="http://sourceforge.net/p/processhacker/code/">Browse source code</a></li>
                            <li><a href="doc/">Source code documentation</a></li>
                        </ul>
                    </div>
                </div>
            </div>

            <div class="summary">
                <dl>
                    <dt>Why is there annoying bug X in Process Hacker? Why is Process Hacker missing feature Y?</dt>
                    <dd>Please report any bugs or feature requests in the <a href="http://wj32.org/processhacker/forums/">forums</a>.</dd>

                    <dt>Is Process Hacker a portable application?</dt>
                    <dd>
                        Yes. In the same directory as <code>ProcessHacker.exe</code>, create a file named <code>ProcessHacker.exe.settings.xml</code>,
                        settings will then be automatically saved here.
                    </dd>

                    <dt>"Process Hacker"? Is this a dangerous "hacking" tool?</dt>
                    <dd>No. Please read about the <a href="http://catb.org/~esr/faqs/hacker-howto.html#what_is">correct definition of "hacker"</a>.</dd>

                    <dt>Why is Process Hacker able to kill processes that no other tools can kill?</dt>
                    <dd>Process Hacker loads a driver that searches memory for an internal Microsoft kernel function and calls it. This special function is not known to be hooked by
                    any malware and security software.</dd>

                    <dt>Process Hacker can kill my anti-virus software! Is this a bug in the anti-virus software?</dt>
                    <dd>No. Please do not report these incidents as bugs because you will be wasting their time.</dd>

                    <dt>Symbols don't work properly!</dt>
                    <dd>
                        Firstly, you need the latest <code>dbghelp.dll</code> version:<br>
                        <br>
                        1) Install the latest Windows SDK. (links are below)<br>
                        2) Open Process Hacker options via the main menu: Hacker &gt; Options<br>
                        3) Click Symbols, and locate <code>dbghelp.dll</code><br>
                        <dl>
                            <dd>
                                Windows XP, Vista and Windows 7 SDK:<br>
                                <code>C:\Program Files\Debugging Tools for Windows (x86)\</code><br><br>
                                Windows 8 or above SDK:<br>
                                32bit: <code>\Program Files (x86)\Windows Kits\8.x\Debuggers\x86\</code><br>
                                64bit: <code>\Program Files (x86)\Windows Kits\8.x\Debuggers\x64\</code><br>
                            </dd>
                        </dl>
                        Secondly, you need to configure the search path. If you don't know what to do, enter:<br>
                        <code>SRV*SOME_FOLDER*http://msdl.microsoft.com/download/symbols</code><br><br>
                        Replace <code>SOME_FOLDER</code> with any folder you can write to, like <code>D:\Symbols</code>.
                        Now you can restart Process Hacker and view full symbols.
                    </dd>

                    <dt>Why can't I build Process Hacker?</dt>
                    <dd>
                        The most likely problem is that you do not have the latest Windows SDK installed.<br>
                        Windows XP, Vista and Windows 7 SDK: <a href="http://msdn.microsoft.com/en-us/windows/bb980924.aspx">Windows SDK</a><br>
                        Windows 7, Windows 8 SDK: <a href="http://msdn.microsoft.com/en-US/windows/hardware/hh852363">Windows 8 SDK</a><br>
                        Windows 7, Windows 8 and 8.1 SDK: <a href="http://msdn.microsoft.com/en-US/windows/desktop/bg162891">Windows 8.1 SDK</a>
                    </dd>

                    <dt>Anti-cheat software reports Process Hacker as a game cheating tool!</dt>
                    <dd>Unfortunately there is nothing much that can be done about this.</dd>

                    <dt>How do I report issues with Anti-cheat software?</dt>
                    <dd>Contact <a href="about.php">dmex</a></dd>
                </dl>
            </div>
        </div>
    </div>
</div>

<?php include "footer.php"; ?>
