<?php

session_start();
//check if there is a cookie on users pc
if(isset($_COOKIE['username']) && isset($_COOKIE['logged'])) {
	
	$cookieUser = $_COOKIE['username'];
	$cookieSalt = $_COOKIE['logged'];
	
	include('includes/config.php');
	
	//match the cookie information with the database information
	$cookieSQL = $link->query("SELECT user_id, username, salt FROM users WHERE username = '".$cookieUser."' AND salt = '" .$cookieSalt. "'");
	
	if(mysqli_num_rows($cookieSQL) === 1) {
	
		$cookieRow = mysqli_fetch_assoc($cookieSQL);
		
		$user_id = intval($cookieRow['user_id']);
		$_SESSION['loggedin'] === TRUE;
		$_SESSION['user_id'] === $user_id;
	} else {
		echo "Invalid cookie information. Please login again.";
	}
}

//if user is logged in
if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id') {

	//set user_id
	$user_id = $_SESSION['user_id'];
	
	include('includes/user.header.php');	

} else {
	include('includes/anon.header.php');
} //close if(isset($_SESSION['loggedin']) && $_SESSION['user_id'] != 'id')
?>

<div class="home_container" style="margin: 80px auto 15px auto; width: 80%;">
<p>
<h3>HireStarts Terms</h3>
</p>
<p>
<b>1.</b> HIRESTARTS.COM reserves the right to modify this Terms of Service ("Agreement") at any time and without prior notice to you. Such modification shall be effective upon being posted on HIRESTARTS.COM ("Website"). You agree to be bound by any changes to this Agreement when you use the website. HIRESTARTS.COM may investigate any activity that may violate this Agreement. We may end your use of the website at any time and at our discretion for any reason. The website may be temporarily unavailable from time to time for maintenance or other reasons. 
</p>
<p>
<b>2.</b> Use of the website is available only to individuals over the age of 18 or at least 14 years of age or older who obtain permission from their parent or legal guardian who accepts this Agreement. By using the website, you represent and warrant that all registration information you submit is truthful and accurate and that you agree to maintain the accuracy of such information. Your account may be deleted without warning, if it is found that you are misrepresenting your age, and you are younger than 18. Your account is solely for personal use, and you shall not authorize others to use your account. You are solely responsible for all content published, displayed, or posted through your account, and for your interactions with other members. 
</p>
<p>
<b>3.</b> By purchasing any services through HIRESTARTS.COM that result in subscription, you hereby agree to adhere to the rules set forth in the Agreement. Should you be found in violation of the Agreement at any time, your subscription may be terminated and nullified without any refund, but such termination shall not preclude other remedies available to HIRESTARTS.COM under these terms or under applicable law. 
</p>
<p>
<b>4.</b> HIRESTARTS.COM is not involved in any financial transaction between any parties who use our sites. There is no agency, partnership, joint venture, employee,employer or franchiser,franchisee relationship between HIRESTARTS.COM and any user of the website. 
</p>
<p>
<b>5.</b> HIRESTARTS.COM cannot and does not control the behavior of the users on our sites. HIRESTARTS.COM does not assume responsibility for the content or context of the user areas. HIRESTARTS.COM does not necessarily endorse, support, sanction, encourage, verify, or agree with the comments, opinions, or statements posted by users in comments, mail, forums, or elsewhere. Any information or material placed by other users, are the views of those who post the statements, and do not necessarily represent the views of HIRESTARTS.COM. You agree that HIRESTARTS.COM is not responsible, and shall have no liability to any person, for any information or materials posted by others, including defamatory, offensive or illicit material. The posting of any such material may be grounds for immediate termination of service without notice for those believed to be responsible. 
</p>
<p>
<b>6.</b> Transmission, distribution or storage of material or conduct in violation of any applicable local, state, federal, or foreign law or regulation is prohibited. This includes without limitation any unauthorized use of material protected by patent, copyright, trademark or other intellectual property right, material that is obscene, defamatory or libelous, constitutes an illegal threat, or violates rights of privacy or publicity, or violates export control laws. You may use the information on our sites, including information from other users, only to the extent necessary to facilitate HIRESTARTS.COM related transactions. 
</p>
<p>
<b>7.</b> HIRESTARTS.COM prohibits the following for any reason, including but is not limited to: obscene, indecent, overtly sexual, or offensive language or images; material that is libelous, abusive, harassing, annoying, or hateful, or that constitutes unauthorized advertising, invades anyone's privacy, or encourages conduct that would constitute a criminal offense, give rise to civil liability, or otherwise violate any local, state, national or international law or regulation; racial, ethnic, religious slurs, derogatory epithets, or other material that is deeply or widely offensive or threatening. 
</p>
<p>
<b>8.</b> HIRESTARTS.COM prohibits the use of another Internet service to: transmit unsolicited bulk or commercial messages or "spam." This includes, but is not limited to, unsolicited advertising, promotional materials or other solicitation material, bulk mailing or commercial advertising, chain mail, informational announcements, charity requests, link to competing service(s), and petitions or signatures; send numerous copies of the same or substantially similar messages, empty messages, or messages which contain no substantive content, or sending very large messages to a recipient that disrupts a server or account. 
</p>
<p>
<b>9.</b> HIRESTARTS.COM has not reviewed and does not endorse the content of all sites linked to or from this website and is not responsible for the content or actions of any other sites linked to or from this website. Linking to any other service or site from this website is at your sole risk. 
</p>
<p>
<b>10.</b> Postings to the website are not private or copyrighted. You grant HIRESTARTS.COM the unrestricted right to use, reproduce, modify, translate, transmit, publish, and distribute any material you post to the website in any medium (now in existence or hereafter developed), and for any purpose, including commercial uses, and to authorize others to do so. You give us a non‘exclusive, worldwide, royalty free, irrevocable, sublicenseable (through multiple tiers) right to exercise all copyright and publicity rights, in any existing or future media, known or unknown, over the material, photographs or User Data displayed in your listings. For the purpose of this Agreement, User Data shall mean all information (if any) submitted by you, (the User) to HIRESTARTS.COM with the exception of trading data, credit card numbers, checking account numbers, etc. Individually Identifiable User Data shall mean that subset of User Data which can be reasonably used to identify a specific individual such as their name, address, phone number, etc. The User Data shall be deemed to be the property of HIRESTARTS.COM. You shall take all reasonable efforts to ensure that it is accurate and complete and not misleading in any way. 
</p>
<p>
<b>11.</b> Other than connecting to HIRESTARTS.COM servers by HTTP requests using a web browser, you may not attempt to gain access to the website by any means including, without limitation, by using administrator passwords or by masquerading as an administrator while using the Website or otherwise. You agree not to disrupt, modify or interfere with the Website or its associated software, hardware and servers in any way, and you agree not to impede or interfere with others' use of the Website. You further agree not to alter or tamper with any information or materials on, or associated with, the website. 
</p>
<p>
<b>12.</b> You agree not to resell or assign your rights or obligations under the Agreement. You also agree not to make any unauthorized commercial use of the Website. 
</p>
<p>
<b>13.</b> If there is a dispute between users on our sites, HIRESTARTS.COM reserves the right, but is under no obligation to monitor disputes or become involved. In the event that you have a dispute with one or more users, you hereby release HIRESTARTS.COM, its officers, employees, agents and successors in rights from claims, demands and damages (actual and consequential) of every kind or nature, known or unknown, suspected and unsuspected, disclosed and undisclosed, arising out of or in any way related to such disputes and/or our service. 
</p>
<p>
<b>14.</b> If HIRESTARTS.COM, in its sole discretion, determines that a violation of the Agreement has occurred, HIRESTARTS.COM may pursue any of its legal remedies, including but not limited to the immediate deletion of any offending material from its site, cancellation of your account and /or the exclusion of any person(s) who are known or believed to have violated any term(s) of this Agreement. HIRESTARTS.COM may also pursue violators with claims that they have violated various criminal and/or civil laws as applied by the relevant jurisdiction. HIRESTARTS.COM will cooperate with any investigation by any Federal, State, or local body or any court or tribunal legitimately exercising its rights. Such cooperation may be without notice to you. If HIRESTARTS.COM believes in its sole discretion that any ad may create liability for HIRESTARTS.COM, HIRESTARTS.COM may take any actions that it believes are prudent or necessary to minimize or eliminate its potential liability, including but not limited to, the release of user information. In such, HIRESTARTS.COM reserves the right to refuse service to anyone at any time, and to remove any listings or any advertisements for any reason, and without notice. 
</p>
<p>
<b>15.</b> The failure of HIRESTARTS.COM to exercise or enforce any right or provision of these Terms of Service shall not constitute a waiver of such right or provision. If any provision of these Terms of Service is found by a court of competent jurisdiction to be invalid, the parties nevertheless agree that the court should endeavor to give effect to the parties intentions as reflected in the provision, and the other provisions of these Terms of Service shall remain in full force and effect. 
</p>
<p>
<b>16.</b> DISCLAIMER OF WARRANTIES. YOU AGREE THAT USE OF THE SERVICE IS AT USER'S SOLE RISK. THE SERVICE IS PROVIDED ON AN "AS IS" AND ON AN "AS AVAILABLE" BASIS. HIRESTARTS.COM EXPRESSLY DISCLAIMS ALL WARRANTIES OF ANY KIND, WHETHER EXPRESS OR IMPLIED, INCLUDING, BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NON,INFRINGEMENT. HIRESTARTS.COM MAKES NO WARRANTY THAT THE SERVICE WILL MEET USER'S REQUIREMENTS, THAT THE SERVICE WILL BE UNINTERRUPTED, TIMELY, SECURE, OR ERROR FREE; NOR DOES HIRESTARTS.COM MAKE ANY WARRANTY AS TO THE RESULTS THAT MAY BE OBTAINED FROM THE USE OF THE SERVICE OR AS TO THE ACCURACY OR RELIABILITY OF ANY INFORMATION OBTAINED FROM THE SERVICE. USER UNDERSTANDS AND AGREES THAT ANY INFORMATION OR MATERIAL AND/OR GOODS OR SERVICES OBTAINED THROUGH THE SERVICE IS DONE AT USER'S OWN DISCRETION AND RISK AND THAT USER WILL BE SOLELY RESPONSIBLE FOR ANY DAMAGE RESULTING FROM ANY TRANSACTION. NO ADVICE OR INFORMATION, WHETHER ORAL OR WRITTEN, OBTAINED BY USER FROM HIRESTARTS.COM OR THROUGH OR FROM THE SERVICE SHALL CREATE ANY WARRANTY NOT EXPRESSLY STATED HEREIN. 
</p>
<p>
<b>17.</b> LIMITATION OF LIABILITY. USER AGREES THAT NEITHER HIRESTARTS.COM NOR ANY OF ITS PROVIDERS OF INFORMATION SHALL BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, CONSEQUENTIAL OR EXEMPLARY DAMAGES, RESULTING FROM THE USE OR THE INABILITY TO USE THE SERVICE OR FOR COST OF PROCUREMENT OF SUBSTITUTE GOODS AND SERVICES OR RESULTING FROM ANY GOODS, DATA, INFORMATION OR SERVICES PURCHASED OR OBTAINED OR MESSAGES RECEIVED OR TRANSACTIONS ENTERED INTO THROUGH OR FROM THE SERVICE OR RESULTING FROM UNAUTHORIZED ACCESS TO OR ALTERATION OF USER'S TRANSMISSIONS OR DATA OR ARISING FROM ANY OTHER MATTER RELATING TO THE SERVICE, INCLUDING BUT NOT LIMITED TO, DAMAGES FOR LOSS OF PROFITS, USE, DATA OR OTHER INTANGIBLE, EVEN IF HIRESTARTS.COM HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. USER FURTHER AGREES THAT HIRESTARTS.COM SHALL NOT BE LIABLE FOR ANY DAMAGES ARISING FROM INTERRUPTION, SUSPENSION OR TERMINATION OF SERVICE, INCLUDING BUT NOT LIMITED TO DIRECT, INDIRECT, INCIDENTAL, SPECIAL CONSEQUENTIAL OR EXEMPLARY DAMAGES, WHETHER SUCH INTERRUPTION, SUSPENSION OR TERMINATION WAS JUSTIFIED OR NOT, NEGLIGENT OR INTENTIONAL, INADVERTENT OR ADVERT. USER AGREES THAT HIRESTARTS.COM SHALL NOT BE RESPONSIBLE OR LIABLE TO USER, OR ANYONE, FOR THE STATEMENTS OR CONDUCT OF ANY THIRD PARTY OF THE SERVICE. SOME STATES DO NOT ALLOW THE EXCLUSION OF IMPLIED WARRANTIES OR THE LIMITATION OR EXCLUSION OF LIABILITY FOR INCIDENTAL OR CONSEQUENTIAL DAMAGES, SO THE ABOVE EXCLUSIONS OR LIMITATIONS MAY NOT APPLY TO YOU. YOU MAY ALSO HAVE OTHER RIGHTS WHICH VARY FROM STATE TO STATE. 
</p>
<p>
<b>18.</b> This Agreement and the relationship between You and HIRESTARTS.COM shall be governed by the laws of the State of Pennsylvania without regard to its conflict of law provisions. You and HIRESTARTS.COM agree to submit to the personal and exclusive jurisdiction of the courts located within Faulkner County, Pennsylvania. 
</p>
<p>
Please report any violations or abuse to the following: 
<a href="mailto:admin@hirestarts.com">admin@hirestarts.COM</a> 
</p>
<p>
<b>Refund Policy:</b><br /> 

Monies used to purchase a subscription are NON-REFUNDABLE except under special circumstances. Any member who deletes their account before their subscription ends will not receive a refund. However, we'll gladly reactivate a members account, if it wasn't deleted by staff, unless it was permanently deleted. Accounts aren't permanently deleted for at least one year after initial deletion, but may be permanently deleted at any time. As stated in the Terms of Service, if you are found to be in violation of the Terms of Service, your subscription(s) may be terminated and nullified without any refund. 
</p>
<p>
<b>(Last Updated: March. 25th, 2012)</b>
</p>
</div>


<?php

include("includes/footer.php");

?>