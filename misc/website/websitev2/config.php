<?php
    // Setup last modified time caching for the current file.
    // This will allow caching client-side for +2 days since last update check, unless edited then it resets.
    $file = __FILE__; 
    $lastmod = date("D, d M Y H:i:s", filemtime($file)); 

    $LATEST_PH_VERSION = "2.27";
    $LATEST_PH_REVISION = "4205";
    $LATEST_PH_RELEASE_DATE = "22nd of January 2012";

    $LATEST_PH_BIN_SIZE = "2.2 MB";
    $LATEST_PH_BIN_SHA1 = "54b285e5e61c8bcde4534b3608c07ebf2c3abc66";
    $LATEST_PH_BIN_MD5 = "3ec09648297299c6a6746f6c1c0a0a79";

    $LATEST_PH_SDK_SIZE = "2.4 MB";
    $LATEST_PH_SDK_SHA1 = "5c4d14521ff19e264334e3875546f31da925bc47";
    $LATEST_PH_SDK_MD5 = "3a374be3ff4ac77ffad51e8c6d6602ae";

    $LATEST_PH_SETUP_SIZE = "1.8 MB";
    $LATEST_PH_SETUP_SHA1 = "b6d90ec86027e474f708b553c7e239dd083c0572";
    $LATEST_PH_SETUP_MD5 = "95779c6aaadafe17706129ae66d6095b";

    $LATEST_PH_SOURCE_SIZE = "2.4 MB";
    $LATEST_PH_SOURCE_SHA1 = "68abb9a6a8a2fd2eec0f5ce86a120819e8e40400";
    $LATEST_PH_SOURCE_MD5 = "92c7be634c9d92c00d831077b26991bd";
    
    // How Many Topics do you want to display?
    $topicnumber = 6;
    
	define('IN_PHPBB', true);
	$phpbb_root_path = './forums/';
	$phpEx = substr(strrchr(__FILE__, '.'), 1);
	
	// import php functions
	include($phpbb_root_path.'config.'.$phpEx); 
	include($phpbb_root_path.'common.'.$phpEx);
	include($phpbb_root_path.'includes/bbcode.'.$phpEx);
	include($phpbb_root_path.'includes/functions_display.'.$phpEx);

    // Start forum session
    $user->session_begin();
    $auth->acl($user->data);
    $user->setup();
    
    // select database tables
    $table_topics = $table_prefix. "topics";
    $table_forums = $table_prefix. "forums";
    $table_posts = $table_prefix. "posts";
    $table_users = $table_prefix. "users";
    $table_sessions = $table_prefix. "sessions";
?>