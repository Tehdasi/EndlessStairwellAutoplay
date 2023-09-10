using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace EndlessStairwellAutoplay
{
	internal class Model
	{
		public WebDriver? driver;
		public int floor;

		public bool
			floorHasRooms = false,
			inFight = false,
			retreatStarted = false,
			atStairs = false,
			atTempRuneShop = false,
			atPermRuneShop = false,
			isDead = false,
			haveKey= false,
			haveRing = false,
			hasGroundFloorJump = false,
			hasFloor49Jump = false,
			hasFloor99Jump = false,
			hasFloor149Jump = false;

		public Num
			level= Num.From(0),
			xp = Num.From(0),
			cocoaHoney = Num.From(0);

		public int
			energy= 0,
			curHealth = 0,
			honey = 0,
			vanillaHoney = 0,
			honeyPlasm = 0,
			roomsAway = 0,
			exploredRooms = 0,
			cocoa = 0,
			cocoaBars = 0;

		public int
			permRuneShopFloor= -1,
			tempRuneShopFloor= -1;

		public int
			redTime = 0,
			greenTime = 0,
			blueTime = 0;

		public int
			redRunes = 0,
			greenRunes = 0,
			blueRunes = 0,
			redRuneFragments = 0,
			greenRuneFragments = 0,
			blueRuneFragments = 0;


		const int numSections = 5;
		const int numRoomsPerSection = 5;


		public int[] roomFloors;

		HashSet<int>[] roomFloorsInternal;

		public Model()
		{
			roomFloors = new int[numSections * numRoomsPerSection];
			roomFloorsInternal = new HashSet<int>[numSections];

			Reset();
		}

		public Model CloneForUi()
		{
			Model m = new Model();
			m.redRunes= redRunes;
			m.greenRunes = greenRunes;
			m.blueRunes = blueRunes;
			return m;
		}

		public void Reset()
		{
			for (int i = 0; i < roomFloors.Length; i++)
				roomFloors[i] = -1;
			for (int i = 0; i < roomFloorsInternal.Length; i++)
				roomFloorsInternal[i] = new HashSet<int>();


			permRuneShopFloor = -1;
			tempRuneShopFloor = -1;

			redRunes = 0;
			greenRunes = 0;
			blueRunes = 0;
		}

		public void Refresh()
		{
			{
				DateTime dt= DateTime.Now;
				string scr = @"
var m= {};
function ap_text( cssSelector )
{
	return document.querySelector( cssSelector ).innerText;
}

function ap_g( cssSelector )
{
	return document.querySelector( cssSelector );
}

function ap_ga( cssSelector )
{
	return document.querySelectorAll( cssSelector );
}
function ap_textRegex( regex, cssSelector )
{
	return  regex.exec( ap_text(cssSelector) );
}
function ap_visible( cssSelector )
{
	var e= ap_g(cssSelector);
	if( e== null )
		return false;
	else
		return window.getComputedStyle(e).display!== 'none';
}
function ap_enabled( cssSelector )
{
	var e= ap_g(cssSelector);
	if( e== null )
		return false;
	else
		return !e.disabled;
}

    {
        var t= ap_text('#currentFloor');	
        if( t.includes('ground') )
            m.floor= 0;
        else
            m.floor= parseInt(/floor (\d+)/.exec(t)[1]);
    }

    m.curHealth= parseInt(ap_g('#healthBarInner').style.width  );
    m.levelRaw= ap_text('#level');
    m.energy= parseInt(ap_textRegex(/(\d+)%/,'#energyBarText')[1]);
	m.xpRaw= ap_text('#totalXP');

    m.redTime= parseInt( ap_text('#buff1') );
    m.greenTime= parseInt( ap_text('#buff2') );
    m.blueTime= parseInt( ap_text('#buff3') );

	m.atStairs= ap_visible('#floorUpButton');

    m.redRuneFragments= parseInt( ap_text('#fragment1Text') );
    m.greenRuneFragments= parseInt( ap_text('#fragment2Text') );
    m.blueRuneFragments= parseInt( ap_text('#fragment3Text') );
    m.haveKey= ap_visible('#blueKeyIcon');
    m.haveRing= ap_visible('#ringIcon');
    m.haveSapphire= ap_visible('#sapphireIcon');
	m.hasGroundFloorJump= ap_visible('#toGroundFloorButton');
	m.hasFloor49Jump= ap_visible('#toFloor49Button');
	m.hasFloor99Jump= ap_visible('#toFloor99Button');
	m.hasFloor149Jump= ap_visible('#toFloor149Button');


    m.honey= parseInt( ap_text('#honey') );
    m.vanillaHoney= parseInt( ap_text('#vanillaHoney') );
	m.cocoaHoneyRaw= ap_text('#cocoaHoney');
	m.honeyPlasm= parseInt( ap_text('#honeyplasm') );
	m.cocoaBars= parseInt( ap_text('#cocoaBars') );

    {
        m.atTempRuneShop= false;
        m.atPermRuneShop= false;
        var t= ap_ga('.smithDiv');
        if( t[0].style.display != 'none' )
        {
            m.tempRuneShopFloor= m.floor;
            m.atTempRuneShop= true;
        }
        if( t[1].style.display != 'none' )
        {
            m.permRuneShopFloor= m.floor;
            m.atPermRuneShop= true;

            var ri= ap_ga('.runeInfo');

            m.redRunes= parseInt(/(\d+)\/(\d+)/.exec( ri[3].innerText )[1]);
            m.greenRunes= parseInt(/(\d+)\/(\d+)/.exec( ri[4].innerText )[1]);
            m.blueRunes= parseInt(/(\d+)\/(\d+)/.exec( ri[5].innerText )[1]);
        }
    }

    if( m.atStairs )
    {
        m.roomsAway= 0;
        m.exploredRooms= 0;
        m.retreatStarted= false;
    }
    else
    {
        var t= ap_textRegex( /have explored (\d+) room.*You are (\d+) room.*difficulty is (\d+)[\,\.]/ , '#info');

        m.exploredRooms= parseInt(t[1]);
        m.roomsAway= parseInt(t[2]);
        m.floorDifficulty= parseInt(t[3]);

        m.retreatStarted= ap_g('#newRoomButton').disabled;
    }

	m.cocoa= parseInt( ap_text('#cocoaHoney') );
	m.floorHasRooms = ap_visible('#toStairwellButton') || ap_visible('#enterFloorButton');

    m.inFight= ap_visible('#monsterDiv');
    m.isDead= ap_visible('#deathDiv');

m;
return m;";
				var res = (Dictionary<string,object>)driver.ExecuteScript( scr );

				foreach(  var i in res )
				{
					Type t = this.GetType();

					if (i.Key.EndsWith("Raw"))
					{
						string fld = i.Key.Substring(0, i.Key.Length - 3);
						var f = t.GetField(fld);
						Num n = Num.Parse((string)i.Value);

						f.SetValue(this, n);
					}
					else
					{
						var fi = t.GetField(i.Key);
						if (fi != null)
						{
							string o = i.Value.GetType().Name;
							switch (o)
							{
								case "Int64": fi.SetValue(this, (int)(long)i.Value); break;
								default:
									fi.SetValue(this, i.Value);
									break;
							}
						}
					}

				}
				var ts = DateTime.Now - dt;
			}


			if( floorHasRooms )
			{
				int section = floor / 50;

				var hs = roomFloorsInternal[section];

				if (!hs.Contains(floor))
				{

					hs.Add(floor);

					var s = roomFloorsInternal[section].Order();

					for (int i = 0; i < s.Count(); i++)
						roomFloors[section * numRoomsPerSection + i] = s.ElementAt(i);
				}
			}
		}

		static public ClickAct BuyCocoaUpgrade( int u )
		{
			return new ClickAct($".cocoaUpgrade:nth-child({u})");
		}

		static public ClickAct FloorUp()
		{
			return new ClickAct("#floorUpButton");
		}
		static public ClickAct FloorDown()
		{
			return new ClickAct("#floorDownButton");
		}

		public ClickAct UseHoney()
		{
			return new ClickAct("#honeyButton1");
		}

		public ClickAct UseVanillaHoney()
		{
			return new ClickAct("#honeyButton2");
		}

		public ClickAct Attack()
		{
			return new ClickAct("#attackButton");
		}

		public ClickAct EnterFloor()
		{
			return new ClickAct("#enterFloorButton");
		}
		public ClickAct NextRoom()
		{
			return new ClickAct("#newRoomButton");
		}
		public ClickAct ToStairs()
		{
			return new ClickAct("#toStairwellButton");
		}

		public ClickAct BuyRedPermRune()
		{
			return new ClickAct(".smithDiv button.smithButton2:nth-child(4)");
		}
		public ClickAct BuyGreenPermRune()
		{
			return new ClickAct(".smithDiv button.smithButton2:nth-child(9)");
		}
		public ClickAct BuyBluePermRune()
		{
			return new ClickAct(".smithDiv button.smithButton2:nth-child(14)");
		}

		public ClickAct BuyRedTempRune()
		{
			return new ClickAct(".smithDiv button.smithButton:nth-child(4)");
		}
		public ClickAct BuyGreenTempRune()
		{
			return new ClickAct(".smithDiv button.smithButton:nth-child(9)");
		}
		public ClickDismissAct AlterPrestige()
		{
			Reset();
			return new ClickDismissAct("#cocoaPrestigeButton");
		}

		public ClickDismissAct CocoaBarPrestige()
		{
			Reset();
			return new ClickDismissAct("#getCocoaBarsButton");
		}

		public static ClickAct Floor99Jump()
		{
			return new ClickAct($"#toFloor99Button");
		}

		public static ClickAct FloorGroundJump()
		{
			return new ClickAct($"#toGroundFloorButton");
		}

		public static ClickAct BuyPlasmUpgrade( int num )
		{
			return new ClickAct($".sharkUpgrade:nth-child({num})");
		}

		public ClickAct BuyBlueTempRune()
		{
			return new ClickAct(".smithDiv button.smithButton:nth-child(14)");
		}


		public ClickAct CloseDeathScreen()
		{
			return new ClickAct("#deathClose");
		}

	}
}
