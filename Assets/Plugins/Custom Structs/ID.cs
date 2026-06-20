//
// Inspired by (and mostly taken from) a StackOverflow answer from "yoyo"
// Page: https://stackoverflow.com/questions/980766/how-do-i-declare-a-nested-enum
//

public struct ID
{
    public static ID none; // Set "null" equivlent of ID to "none"
    private readonly uint mID;

    // Setup a "child" element by adding its parent's ID to the left of the child ID
    public ID this[int childID]
    {
        get { return new ID((mID << 8) | (uint)childID); }
    }

    // Get the "Parent" element of this ID
    public ID super
    {
        get { return new ID(mID >> 8); }
    }

    // Check if the caller is part of the group (or sub-group) given
    public bool isItA(ID super)
    {
        return (this != none) && ((this.super == super) || this.super.isItA(super));
    }

    // Enables any int or anything larger to be used without needing a cast to uint every time
    public static implicit operator ID(int id) 
    {
        if (id == 0) { // Prevents overlapping with the "null" equivlent defined earlier
            throw new System.InvalidCastException("Top level ID can't be 0");
        }
        return new ID((uint)id);
    }

    // Makes ID compaire the mID instead of the entire variable when used in == or != comparisons
    public static bool operator ==(ID a, ID b) { return a.mID == b.mID; }
    public static bool operator !=(ID a, ID b) { return a.mID != b.mID; }

    // Overrides what the game does when compairing 2 IDs
    public override bool Equals(object obj)
    {
        if (obj is ID) // Check if its an ID (to be safe)
            return ((ID)obj).mID == mID;
        else
            return false;
    }
    // Overrides GetHashCode too (IDK why but it's required when you override Equals)
    public override int GetHashCode() { return (int)mID; }

    // Allows for recursive-like interaction by enabling the ID to be passed down (normally in an altered state)
    private ID(uint id) {
        mID = id;
    }
}
