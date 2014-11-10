import processing.core.*; 
import processing.data.*; 
import processing.event.*; 
import processing.opengl.*; 

import arb.soundcipher.*; 
import java.util.Properties; 
import java.util.Map; 
import java.util.Collections; 

import java.util.HashMap; 
import java.util.ArrayList; 
import java.io.File; 
import java.io.BufferedReader; 
import java.io.PrintWriter; 
import java.io.InputStream; 
import java.io.OutputStream; 
import java.io.IOException; 

public class MAIN extends PApplet {

//Welcome to SoniTrak, a perceptualization system by MANOS and MAARTEN






float currentInstrument = 0;

float TransitAngle = 79.0f;
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

public void setup()
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

  //props.put("Trajectory", "Saturn"); //disable this when exporting  
  
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
  
    TransitAngle = 79.0f;    

    String[] RiseTime = split(trajectory.getRiseAzDate(), ":");
    String[] TransitTime = split(trajectory.getTransitAltDate(), ":");
    String[] SetTime = split(trajectory.getSetAzDate(), ":");
  
    RiseHour = PApplet.parseInt(RiseTime[0]);
    RiseMinute = PApplet.parseInt(RiseTime[1]);   
    RiseAngle = trajectory.getRiseAzAngle();
    
    SetHour = PApplet.parseInt(SetTime[0]);
    SetMinute = PApplet.parseInt(SetTime[1]);    
    SetAngle = trajectory.getSetAzAngle();
    
    TransitHour = PApplet.parseInt(TransitTime[0]);
    TransitMinute = PApplet.parseInt(TransitTime[1]);;    
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

public void draw()
{
  //SET TEMPO and PAN
  sc.pan(function.getPan());
  frameRate((Integer)FPSTable.get(trajectory.Name)); //dictated by distance to object
  
  //SET ANGLE (chord)
  sc.playNote(60, 130, 2.0f);
  sc.playNote(function.getAngle(chour, cminute), 120, 6.0f);
  
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

public void keyPressed()
{
  if(keyCode == 90)
    noLoop();
  
  if(keyCode == 88)
    loop();
}


public String param(String id) {
    if (online) return super.param(id);
    else return (String)params.get(id);
}


class Function
{  
  int pan1 = 0;
  
  Function () {
 
  }

  public Properties loadCommandLine () {

    Properties props = new Properties();

    for (String arg : args) {
      String[] parsed = arg.split("=", 2);
      if (parsed.length == 2)
        props.setProperty(parsed[0], parsed[1]);
    }

    return props;
  }

  //tone
  public float modeQuantize(float pitch, float[] mode, int keyOffset) {
    pitch = round(pitch / 2);    
    boolean inScale = false;    
    while (!inScale) 
    {
      for (int i=0; i<mode.length; i++) 
      {        
        if ((pitch - keyOffset) % 12 == mode[i]) 
          inScale = true;
        if (!inScale)
            pitch++;
      }
    }
    
    return pitch;
  }

  public float getAngle(int hour, int minute)
  {   
    float diff = ((TransitHour * 60) + TransitMinute) - ((RiseHour * 60) + RiseMinute);
    float MinuteAngle = TransitAngle / diff; 

    if(Rising)
    {
      CurrentAngle = ((hour * 60) + minute) * MinuteAngle;
      RisingAngles.add(CurrentAngle);
    }
    else
    {
       //lol
       if(SetI != RisingAngles.size())
       {
         CurrentAngle = RisingAngles.get(SetI);
         SetI++;
       }
    }

    return CurrentAngle = modeQuantize(CurrentAngle, sc.MAJOR, keyOffset);
  }
  float pan2;
  public int getPan()
  {             
    float PanDiff = 127.0f / trajectory.getSpeed();

    pan2 += PanDiff;

    if (pan2 >= 127.0f)
      pan2 -= PanDiff;
    
    return PApplet.parseInt(pan2);
  }

  public void readFile(String file)
  {    
    String[] lines =  loadStrings("data/"+file+".txt");   
    trajectory = new Trajectory(file, lines.length);
    
    for (int o = 0; o < lines.length; o++ )
    {    
      String line = lines[o];
      String[] words = split(line, ',');                           

      trajectory.setDate(words[0]);

      trajectory.setRiseAzDate(words[1]);
      trajectory.setRiseAzAngle(PApplet.parseInt(words[2]));

      trajectory.setTransitAltDate(words[3]);
      trajectory.setTransitAltAngle(PApplet.parseInt(words[4]));

      trajectory.setSetAzDate(words[5]);
      trajectory.setSetAzAngle(PApplet.parseInt(words[6]));

      trajectory.Index++;
    }    

    trajectory.Index = 0;     
  }
}

class Trajectory
{
  public int Index;
  public int Length;
  
 /* 
  * perihelion range for solar objects
  * 
  * pluto (max) ~ 4436 (sounds very 'mumbly')
  * mars (min) ~ 206 (so this sounds crisp)
  *
  * getting perihelion of stars may be a bit confusing, but distance will do nicely in that case 
  * the number is quite arbitrary anyway...
  */
  
  //Determines if the transit is north or south of the observers meridian (NOT USED)
  public boolean North;
  private int Speed;
  public String Name;

  private String[] Dates;

  //RISE
  private String[] RiseAzDate;
  private int[] RizeAzAngle;

  //TRANSIT  
  private String[] TransitAltDate;
  private int[] TransitAltAngle;

  //SET
  private String[] SetAzDate;
  private int[] SetAzAngle;

  Trajectory(String name, int size)
  {
    this.Name = name;
    this.Dates = new String[size];
    this.RiseAzDate = new String[size];
    this.RizeAzAngle = new int[size];
    this.TransitAltDate = new String[size];
    this.TransitAltAngle = new int[size];
    this.SetAzDate = new String[size];
    this.SetAzAngle = new int[size];

    this.Length = size;
  }

  public String getDate()
  {
    return this.Dates[this.Index];
  }

  public String getRiseAzDate()
  {
    return this.RiseAzDate[this.Index];
  }

  public int getRiseAzAngle()
  {
    return this.RizeAzAngle[this.Index];
  }

  public String getTransitAltDate()
  {
    return this.TransitAltDate[this.Index];
  }

  public int getTransitAltAngle()
  {
    return this.TransitAltAngle[this.Index];
  }

  public String getSetAzDate()
  {
    return this.SetAzDate[this.Index];
  }

  public int getSetAzAngle()
  {
    return this.SetAzAngle[this.Index];
  }

  public void setDate(String d)
  {
    this.Dates[this.Index] = d;
  }

  public void setRiseAzDate(String d)
  {
    this.RiseAzDate[this.Index] = d;
  }

  public void setRiseAzAngle(int i)
  {
    this.RizeAzAngle[this.Index] = i;
  }

  public void setTransitAltDate(String d)
  {
    this.TransitAltDate[this.Index] = d;
  }

  public void setTransitAltAngle(int i)
  {
    this.TransitAltAngle[this.Index] = i;
  }

  public void setSetAzDate(String d)
  {
    this.SetAzDate[this.Index] = d;
  }

  public void setSetAzAngle(int i)
  {
    this.SetAzAngle[this.Index] = i;
  }
  
  public void setSpeed(int i)
  {
    //snelheid is afhankelijk van cminute            
    int RiseMinutes = (RiseHour * 60) + RiseMinute;
    int SetMinutes = (SetHour * 60) + SetMinute;
    
    int TotalMinutes = abs(SetMinutes - RiseMinutes);
    
    //speed is het aantal minuten per frame, framerate zelf is niet belangrijk
    int D = TotalMinutes / i; //dit klopt nog niet, ik mis ~ 15 - 25 frames steeds
    
    this.Speed = D;
  }
  
  public int getSpeed()
  {
    return this.Speed;
  }
}

  static public void main(String[] passedArgs) {
    String[] appletArgs = new String[] { "MAIN" };
    if (passedArgs != null) {
      PApplet.main(concat(appletArgs, passedArgs));
    } else {
      PApplet.main(appletArgs);
    }
  }
}
