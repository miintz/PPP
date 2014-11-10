//Welcome to SoniTrak, a perceptualization system by MANOS and MAARTEN

import arb.soundcipher.*;
import java.util.Properties;
import java.util.Map;
import java.util.Collections;

float currentInstrument = 0;

float TransitAngle = 79.0;
float CurrentAngle; //min 0 max TransitAngle
float CurrentRotation;

int RiseHour = 6;
int RiseMinute = 30;
int RiseAngle = 108;

int SetHour = 21;
int SetMinute = 30;
int SetAngle = 252;

int TransitHour = 13;
int TransitMinute = 30;

int cminute = minute();
int chour = hour();

int keyOffset = 20;

boolean Rising = true;

//lol
ArrayList<Float> RisingAngles = new ArrayList<Float>();
int SetI = 0;

static HashMap params = new HashMap();
static HashMap Trajectory = new HashMap();
static HashMap FPSTable = new HashMap<String, Integer>();

Function function;
Trajectory trajectory;

SoundCipher sc = new SoundCipher();

void setup()
{  
  function = new Function();
  
  Properties props = function.loadCommandLine();

  //silly list for speed
  FPSTable.put("Pluto", 1);
  FPSTable.put("Neptune", 2);
  FPSTable.put("Uranus", 3);
  FPSTable.put("Saturn", 4);
  FPSTable.put("Jupiter", 5);
  FPSTable.put("Mars", 6);
  FPSTable.put("Venus", 7);
  FPSTable.put("Mercury", 8);

  props.put("Trajectory", "Pluto"); //disable this when exporting  
  
  if (props.get("Trajectory") == null)
  {
    println("Please set a trajectory e.g. trajectory=Jupiter (case sensitive)");
    exit();
  }
  else
  {
    function.readFile((String)props.get("Trajectory"));    
    
    size(700, 300);
    background(0);

    textSize(13);
    text("Welcome to SoniTrak (by MANOS and MAARTEN)", 30, 40);

    props.remove("--sketch-path"); //hebben we niet nodig
   
    //now set find the starting date of the object from Trajectory        
    String curm = "";
    switch(month())
    {
      case 1: curm = "Jan"; break; case 2: curm = "Feb"; break; case 3: curm = "Mar"; break; 
      case 4: curm = "Apr"; break; case 5: curm = "May"; break; case 6: curm = "Jun"; break; 
      case 7: curm = "Jul"; break; case 8: curm = "Aug"; break; case 9: curm = "Sep"; break; 
      case 10: curm = "Oct";break; case 11:curm = "Nov"; break; case 12:curm = "Dec"; break;   
    }    
    
    String date = "";
    if(day() < 10)
       date = year() + " " + curm + " 0" + day();
    else
       date = year() + " " + curm + " " + day();
    
    boolean notfound = true;
    
    while(notfound)
    {                 
      if(trajectory.getDate().equals(date))                
        notfound = false; //break, internal timer is fine now
      else                
        trajectory.Index++; //set the internal timer one step further
    }     
  
    TransitAngle = 79.0;    

    String[] RiseTime = split(trajectory.getRiseAzDate(), ":");
    String[] TransitTime = split(trajectory.getTransitAltDate(), ":");
    String[] SetTime = split(trajectory.getSetAzDate(), ":");
  
    RiseHour = int(RiseTime[0]);
    RiseMinute = int(RiseTime[1]);   
    RiseAngle = trajectory.getRiseAzAngle();
    
    SetHour = int(SetTime[0]);
    SetMinute = int(SetTime[1]);    
    SetAngle = trajectory.getSetAzAngle();
    
    TransitHour = int(TransitTime[0]);
    TransitMinute = int(TransitTime[1]);;    
    TransitAngle = trajectory.getTransitAltAngle();   
    
    chour = RiseHour;
    cminute = RiseMinute;
    
    textSize(11);
    //print out the props for some easy viewing and debug thingamajigs
    props.put("Date", date);
    
    int i = 1;
    if (props.size() != 0)
    {
      for (Map.Entry prop : props.entrySet ())
      {
        text(prop.getKey() + " : " + prop.getValue(), 30, 60 + (i * 20));
        i++;
      } 
    }   
   
    trajectory.setSpeed((Integer)FPSTable.get(trajectory.Name));   
    println(trajectory.Name + " RISES AT : "  + chour + ":" + cminute); 
  }
}

void draw()
{
  //SET TEMPO and PAN
  sc.pan(function.getPan());
  frameRate((Integer)FPSTable.get(trajectory.Name)); //dictated by distance to object
  
  //SET ANGLE (chord)
  //sc.playNote(60, 130, 2.0);
  //sc.playNote(function.getAngle(chour, cminute), 120, 6.0);
  
  float[] cord = {60, function.getAngle(chour, cminute)};
  
  sc.playChord(cord,100,2.0);
  
  if(abs(chour - SetHour) == 0 && abs(cminute - SetMinute) < (Integer)FPSTable.get(trajectory.Name) && !Rising) //einde
  {    
    println(trajectory.Name + " REACHED END : "  + chour + ":" + cminute + " - FRAMES - " + frameCount);
    noLoop();
  }
  else if((chour >= TransitHour && cminute >= TransitMinute) && Rising) //transit TIME not ANGLE
  {
    println(trajectory.Name + " REACHED TRANSIT AT : " + chour + ":" + cminute + " - FRAMES - " + frameCount);
    Rising = false;
    Collections.reverse(RisingAngles);  
  }
  else
  { //dit moet eigenlijk met modulo werken, want 60 is niet altijd deelbaar door de FPSTable, daarom vallen er minuten af en klinkt het raar    
    if (cminute > 59)    
    {
      int remainder = cminute % 60;
      
      if (chour == 23)           
        chour = 0;      
      else
        chour++;
  
      if(remainder != 0)
        cminute = remainder;
      else
        cminute = 0;
        
    } else
      cminute += (Integer)FPSTable.get(trajectory.Name);  
  }
}

void keyPressed()
{
  if(keyCode == 90)
    noLoop();
  
  if(keyCode == 88)
    loop();
}


String param(String id) {
    if (online) return super.param(id);
    else return (String)params.get(id);
}


