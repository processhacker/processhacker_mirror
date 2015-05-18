<?php include "config.php";

header('Content-Type: application/xml; charset=UTF-8');

echo "<?xml version=\"1.0\" encoding=\"UTF-8\"?>".PHP_EOL;
echo "<latest>".PHP_EOL;
echo "<ver>".$LATEST_PH_VERSION."</ver>".PHP_EOL;
echo "<rev>".$LATEST_PH_BUILD."</rev>".PHP_EOL;
echo "<sha1>".$LATEST_PH_SETUP_SHA1."</sha1>".PHP_EOL;
echo "<reldate>".$LATEST_PH_RELEASE_DATE."</reldate>".PHP_EOL;
echo "<size>".$LATEST_PH_SETUP_SIZE."</size>".PHP_EOL;
echo "<relnotes>".htmlspecialchars($LATEST_PH_RELEASE_NEWS)."</relnotes>".PHP_EOL;
echo "<setupurl>".htmlspecialchars($LATEST_PH_RELEASE_URL)."</setupurl>".PHP_EOL;
echo "</latest>";

file_put_contents("update-count.txt", "1", FILE_APPEND);
?>